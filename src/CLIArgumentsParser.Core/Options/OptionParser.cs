using System;
using System.Collections.Generic;
using System.Linq;
using CLIArgumentsParser.Core.Parsing;

namespace CLIArgumentsParser.Core.Options
{
	internal class OptionParser : ModelParser<Option>
	{
		internal OptionParser(Option model) : base(model, new TokenGenerator(new List<string>() { Option.OPTION_IDENTIFIER }))
		{
		}

		/// <summary>
		/// True if key is valid compared to attribute definition
		/// </summary>
		protected override bool IsKeyValid(string key, out string errorMessage)
		{
			errorMessage = string.Empty;

			var keyWithoutIdentifier = key.StartsWith(Option.OPTION_IDENTIFIER) ? key.Replace(Option.OPTION_IDENTIFIER, "") : key;
			if (keyWithoutIdentifier.ToLowerInvariant() == this._Model.Code.ToLowerInvariant() || keyWithoutIdentifier.ToLowerInvariant() == this._Model.LongCode.ToLowerInvariant())
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
			if (targetTokens.Count() != 1)
				throw new InvalidCLIArgumentException($"Invalid Argument: Wrong number of recognized tokens for Option {String.Join(" ", targetTokens.Select(x => x.Name + " " + x.Value))}",
														String.Join(" ", targetTokens.Select(x => x.Name + " " + x.Value)));
			if (!String.IsNullOrWhiteSpace(targetTokens.ToList()[0].Value))
			{
				//We need to manage the arguments
				var outputValue = targetTokens.ToList()[0].Value.Trim();
				// validate output value
				if (this._Model.AvailableValues.Count > 0 && !this._Model.AvailableValues.Contains(outputValue))
					throw new InvalidCLIArgumentException($"Invalid argument: {outputValue} is not a valid value for option {targetTokens.First().Name}. Admitted values: {string.Join("; ", this._Model.AvailableValues)}", targetTokens.First().Name);

				return outputValue;
			}

			if (String.IsNullOrWhiteSpace(targetTokens.ToList()[0].Value) && this._Model.DefaultValue != null)
				return this._Model.DefaultValue;

			// if option is there, this means option enabled
			return true;
		}

	}
}
