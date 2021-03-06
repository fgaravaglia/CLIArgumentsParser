﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIArgumentsParser.Core.Options
{
    /// <summary>
    /// Class to map a simple option
    /// </summary>
    public class Option : ICLIArgumentModel
    {
        /// <summary>
        /// char to use to start an option
        /// </summary>
        internal const string OPTION_IDENTIFIER = "--";
        /// <summary>
        /// char to use to separate key from value
        /// </summary>
        internal const string OPTION_VALUE_SEPARATOR = "=";

        /// <summary>
        /// Code of the option, without option initializer char
        /// </summary>
        /// <example>o, src, i</example>
        public string Code { get; private set; }
        /// <summary>
        /// Type of option value
        /// </summary>
        public Type TargetType { get; private set; }
        /// <summary>
        /// Description of the option, like helper text
        /// </summary>
        public string HelpText { get; private set; }
        /// <summary>
        /// Gets the type of argument (Verb, Option)
        /// </summary>
        public string ArgumentType { get { return "OPTION"; } }
        /// <summary>
        /// Gets the list of examples for the usages
        /// </summary>
        public IEnumerable<CLIUsageExample> Examples { get; private set; }
        /// <summary>
        /// Long Code of the option, without option initializer char
        /// </summary>
        /// <example>output, source, input</example>
        public string LongCode { get; private set; }
        /// <summary>
        /// TRUE if the option is mandatory
        /// </summary>
        public bool Mandatory { get; private set; }
        /// <summary>
        /// Default value to apply to option
        /// </summary>
        public object DefaultValue { get; private set; }
        /// <summary>
        /// Code of the option, without option initializer char
        /// </summary>
        /// <example>o, src, i</example>
        public List<string> AvailableValues { get; private set; }
        /// <summary>
        /// In case of Complex Option (like an oject), sets the inner options
        /// </summary>
        public List<Option> Options { get; private set; }
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="longCode"></param>
        /// <param name="description">help text for the option, useful for helper</param>
        /// <exception cref="System.ArgumentException">Thrown if one of the params is null, empty or whitespace.</exception>
        private Option(string code, string longCode, string description)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException(nameof(code));
            if (string.IsNullOrWhiteSpace(longCode)) throw new ArgumentException(nameof(longCode));
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException(nameof(description));

            this.Code = code;
            this.LongCode = longCode;
            this.HelpText = description;
            this.Mandatory = false;
            this.DefaultValue = null;
            this.AvailableValues = new List<string>();
            this.Examples = new List<CLIUsageExample>();
            this.Options = new List<Option>();
        }

        /// <summary>
        /// Instances the entity from its definition stored in custom attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <exception cref="System.ArgumentException">Thrown if one of the params is null</exception>
        static Option MapFromAttribute(OptionDefinitionAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            var opt = new Option(attribute.Code, attribute.LongCode, attribute.Description);
            opt.Mandatory = attribute.Mandatory;
            opt.DefaultValue = attribute.DefaultValue;

            return opt;
        }
        /// <summary>
        /// Instances the entity from its definition stored in custom attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <exception cref="System.ArgumentException">Thrown if one of the params is null</exception>
        internal static Option FromAttribute(OptionDefinitionAttribute attribute)
        {
            var opt = MapFromAttribute(attribute);

            // check type
            LOVOptionDefinitionAttribute lovAttr = attribute as LOVOptionDefinitionAttribute;
            if (lovAttr != null)
                return FromLOVAttribute((LOVOptionDefinitionAttribute)attribute);

            return opt;
        }
        /// <summary>
        /// Instances the entity from its definition stored in custom attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <exception cref="System.ArgumentException">Thrown if one of the params is null</exception>
        internal static Option FromLOVAttribute(LOVOptionDefinitionAttribute attribute)
        {
            var opt = MapFromAttribute(attribute);
            attribute.AvailableValues.ForEach(x => opt.AvailableValues.Add(x));
            return opt;
        }
        /// <summary>
        /// sets the potperty type marked by this option
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal Option OnTargetProperty(Type target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            this.TargetType = target;
            return this;
        }
        /// <summary>
        /// Adds the example
        /// </summary>
        /// <param name="example"></param>
        /// <returns></returns>
        internal Option AddExample(CLIUsageExample example)
        {
            if (example == null)
                throw new ArgumentNullException(nameof(example));

            var examples = this.Examples.ToList();
            examples.Add(example);
            this.Examples = examples;
            return this;
        }
    }
}
