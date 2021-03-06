﻿using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CLIArgumentsParser.Core.Verbs
{
    internal class VerbParser : ModelParser<Verb>
    {
        /// <summary>
        /// default constructor
        /// </summary>
        internal VerbParser(Verb model) : base(model, new TokenGenerator(new List<string>() { Option.OPTION_IDENTIFIER }))
        {
        }

        /// <summary>
        /// True if key is valid compared to attribute definition
        /// </summary>
        internal override bool IsKeyValid(string key, out string errorMessage)
        {
            errorMessage = String.Empty;

            // define the key identifier
            string keyWithoutIdentifier = key;
            if (key.StartsWith(Option.OPTION_IDENTIFIER))
                keyWithoutIdentifier = key.Substring(2, key.Length - 2).Trim();
            else if (key.StartsWith("-"))
                keyWithoutIdentifier = key.Substring(1, key.Length - 1).Trim();

            if (this._Model.Name == keyWithoutIdentifier || this._Model.Options.Exists(x => x.Code == keyWithoutIdentifier || x.LongCode == keyWithoutIdentifier))
                return true;

            errorMessage = "Unrecognized option code";
            return false;
        }
        /// <summary>
        /// Executes the parse for the given key/arguments
        /// </summary>
        /// <returns></returns>
        protected override object ParseFromTokens(IEnumerable<Token> targetTokens)
        {
            // instance the value
            var returnValue = Activator.CreateInstance(this._Model.TargetType);

            // no token: return the empty option class
            if (!targetTokens.Any())
                return returnValue;

            // I need to parse the options
            var tokens = targetTokens.Skip(1).ToList();

            // get properties to map
            var optionDefinitions = OptionDefinitionAttributeHelper.ExtractPropertiesMarkedWithOptionAttribute(this._Model.TargetType);
            Dictionary<PropertyInfo, OptionDefinitionAttribute> alreadyConsidered = new Dictionary<PropertyInfo, OptionDefinitionAttribute>();
            foreach (var token in tokens)
            {
                // find corresponding attribute
                var attribute = AssertOptionAttributeExists(targetTokens.First().Name, token, optionDefinitions);
                var targetProperty = optionDefinitions.Single(x => x.Value.Code == token.Name || x.Value.LongCode == token.Name).Key;

                // parse the value
                var optionParser = new OptionParser(Option.FromAttribute(attribute).OnTargetProperty(targetProperty.PropertyType));
                var outputValue = optionParser.Parse(token.AsNaturalString());

                // update the value
                targetProperty.SetValue(returnValue, outputValue);

                // already managed
                alreadyConsidered.Add(targetProperty, attribute);
            }

            // considered not managed
            foreach (var opt in optionDefinitions)
            {
                var definition = alreadyConsidered.Values.SingleOrDefault(x => x.Code == opt.Value.Code);
                if (definition != null)
                    continue;
                if (opt.Value.Mandatory)
                    throw new InvalidCLIArgumentException($"Mandatory option {opt.Value.Code} not found in {this._Model.TargetType.Name}", "opt.Value.Code");
                if (opt.Value.DefaultValue != null)
                {
                    var targetProperty = opt.Key;
                    targetProperty.SetValue(returnValue, opt.Value.DefaultValue);
                }
            }

            return returnValue;
        }

        private OptionDefinitionAttribute AssertOptionAttributeExists(string verb, Token token, Dictionary<PropertyInfo, OptionDefinitionAttribute> optionDefinitions)
        {
            // find correspongin attribute
            var attribute = optionDefinitions.Values.SingleOrDefault(x => x.Code == token.Name || x.LongCode == token.Name);
            if (attribute == null)
                throw new InvalidCLIArgumentException($"Unable to find option {token.Name} for verb {verb}", verb);

            return attribute;
        }
    }
}
