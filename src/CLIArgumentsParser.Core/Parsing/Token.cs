using System;
using CLIArgumentsParser.Core.Options;

namespace CLIArgumentsParser.Core.Parsing
{
	/// <summary>
	/// BAse element to analyze strings and recognize the syntax
	/// </summary>
	public class Token
	{
		/// <summary>
		/// Name of token
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// value of token
		/// </summary>
		public string Value { get; private set; }
		/// <summary>
		///  separator for name/value
		/// </summary>
		public string Separator { get; private set; }
		/// <summary>
		/// gets the string representation in natural way
		/// </summary>
		/// <returns></returns>
		public string AsNaturalString()
		{
			return $"{Name}{Separator}{Value}";
		}
		/// <summary>
		/// Generates the token
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Token New(string name, string value)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			return new Token()
			{
				Name = name,
				Value = value,
				Separator = Option.OPTION_VALUE_SEPARATOR
			};
		}
	}
}
