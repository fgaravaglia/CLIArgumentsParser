using System;
using System.Reflection;
using CLIArgumentsParser.Core.Parsing;

namespace CLIArgumentsParser.Core.Verbs
{
	internal class VerbParser : ArgumentParser<VerbDefinitionAttribute, Verb>
	{
		internal VerbParser(VerbDefinitionAttribute attribute) : base(attribute)
		{
		}

		/// <summary>
		/// True if key is valid compared to attribute definition
		/// </summary>
		protected override bool IsKeyValid(string key, out string errorMessage)
		{
			errorMessage = String.Empty;

			if (key.StartsWith("-"))
			{
				errorMessage = "A verb cannot start with '-'";
				return false;
			}

			if (key.ToLowerInvariant() == this._Attribute.Name.ToLowerInvariant())
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
			// instance the value
			var returnValue = Activator.CreateInstance(targetReturnType);
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
