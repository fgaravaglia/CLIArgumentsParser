using System;
using System.Collections.Generic;
using System.Linq;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Parsing;

namespace CLIArgumentsParser.Core.Verbs
{
	internal class VerbParser : ArgumentParser<VerbDefinitionAttribute, Verb>
	{
		/// <summary>
		/// default constructor
		/// </summary>
		/// <param name="attribute"></param>
		/// <param name="targetType"></param>
		internal VerbParser(VerbDefinitionAttribute attribute, Type targetType) : base(attribute, new TokenGenerator(new List<string>() { "--" }), targetType)
		{
			this._Model = Verb.FromAttribute(attribute);
			var optionDefinitions = OptionDefinitionAttributeHelper.ExtractPropertiesMarkedWithOptionAttribute(this._TargetType);
			foreach (var opt in optionDefinitions)
			{
				this._Model.AddOptionFromAttribute(opt.Value);
			}
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
			var returnValue = Activator.CreateInstance(this._TargetType);

			// crete the verb option
			if (targetTokens.Count() > 0)
			{
				// I need to parse the options
				var tokens = targetTokens.Skip(1).ToList();
				// get properties to map
				var optionDefinitions = OptionDefinitionAttributeHelper.ExtractPropertiesMarkedWithOptionAttribute(this._TargetType);
				foreach (var token in tokens)
				{
					// find correspongin attribute
					var attribute = optionDefinitions.Values.SingleOrDefault(x => x.Code == token.Name || x.LongCode == token.Name);
					if (attribute == null)
						throw new InvalidCLIArgumentException($"Unable to find option {token.Name} for verb {targetTokens.First().Name}", targetTokens.First().Name);
					var targetProperty = optionDefinitions.Single(x => x.Value.Code == token.Name || x.Value.LongCode == token.Name).Key;
					// parse the value
					var optionParser = new OptionParser(attribute, targetProperty.PropertyType);
					var outputValue = optionParser.Parse(token.AsNaturalString());
					// update the value
					targetProperty.SetValue(returnValue, outputValue);
				}
			}
			return returnValue;
		}

		/// <summary>
		/// maps the definition of argument coming out from attribute into model
		/// </summary>
		/// <param name="attribute"></param>
		/// <returns></returns>
		protected override Verb FromAttribute(VerbDefinitionAttribute attribute)
		{
			return Verb.FromAttribute(this._Attribute);
		}
	}
}
