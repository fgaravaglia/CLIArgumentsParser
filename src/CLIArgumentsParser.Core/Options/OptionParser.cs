using System;
using System.Reflection;
using CLIArgumentsParser.Core.Parsing;

namespace CLIArgumentsParser.Core.Options
{
	internal class OptionParser : ArgumentParser<OptionDefinitionAttribute, Option>
	{
		internal OptionParser(OptionDefinitionAttribute attribute) : base(attribute)
		{
		}

		/// <summary>
		/// True if key is valid compared to attribute definition
		/// </summary>
		protected override bool IsKeyValid(string key, out string errorMessage)
		{
			errorMessage = string.Empty;
			//if (!key.StartsWith("-"))
			//{
			//	errorMessage = "Wrong format: the option must to start with '-'";
			//	return false;
			//}

			if (key.ToLowerInvariant() == this._Attribute.Code.ToLowerInvariant() || key.ToLowerInvariant() == this._Attribute.LongCode.ToLowerInvariant())
				return true;

			errorMessage = "Unrecognized option code";
			return false;
		}
		/// <summary>
		/// Executes the parse for the given key/arguments
		/// </summary>
		/// <param name="key"></param>
		/// <param name="keyArguments"></param>
		/// <param name="targetReturnType"></param>
		/// <returns></returns>
		protected override object ParseFromKey(string key, string keyArguments, Type targetReturnType)
		{
			if (!String.IsNullOrWhiteSpace(keyArguments))
			{
				//We need to manage the arguments
				return keyArguments.Trim();
			}

			// if there is a default and no argument
			if (String.IsNullOrWhiteSpace(keyArguments) && this._Attribute.DefaultValue != null)
				return this._Attribute.DefaultValue;

			// if option is there, this means option enabled
			return true;
		}
		/// <summary>
		/// maps the definition of argument coming out from attribute into model
		/// </summary>
		/// <param name="attribute"></param>
		/// <returns></returns>
		protected override Option FromAttribute(OptionDefinitionAttribute attribute)
		{
			return Option.FromAttribute(this._Attribute);
		}
	}
}
