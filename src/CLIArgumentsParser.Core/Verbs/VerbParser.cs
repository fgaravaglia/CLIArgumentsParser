using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Parsing;

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
		protected override bool IsKeyValid(string key, out string errorMessage)
		{
			errorMessage = String.Empty;

			string keyWithoutIdentifier = key.StartsWith("--") ? key.Substring(2, key.Length - 2).Trim()
											: key.StartsWith("-") ? key.Substring(1, key.Length - 1).Trim() : key;
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
			if (targetTokens.Count() == 0)
				throw new InvalidCLIArgumentException($"NO Token Found for Verb", "Verb");

			// instance the value
			var returnValue = Activator.CreateInstance(this._Model.TargetType);

			// crete the verb option
			if (targetTokens.Count() > 0)
			{
				// I need to parse the options
				var tokens = targetTokens.Skip(1).ToList();
				// get properties to map
				var optionDefinitions = OptionDefinitionAttributeHelper.ExtractPropertiesMarkedWithOptionAttribute(this._Model.TargetType);
				Dictionary<PropertyInfo, OptionDefinitionAttribute> alreadyConsidered = new Dictionary<PropertyInfo, OptionDefinitionAttribute>();
				foreach (var token in tokens)
				{
					// find correspongin attribute
					var attribute = optionDefinitions.Values.SingleOrDefault(x => x.Code == token.Name || x.LongCode == token.Name);
					if (attribute == null)
						throw new InvalidCLIArgumentException($"Unable to find option {token.Name} for verb {targetTokens.First().Name}", targetTokens.First().Name);
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
			}
			return returnValue;
		}

	}
}
