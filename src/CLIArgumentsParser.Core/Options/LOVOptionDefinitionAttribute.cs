using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIArgumentsParser.Core.Options
{
	/// <summary>
	/// Attribute to decorate a property as a CLI Option
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class LOVOptionDefinitionAttribute : OptionDefinitionAttribute
	{
		/// <summary>
		/// Code of the option, without option initializer char
		/// </summary>
		/// <example>o, src, i</example>
		public List<string> AvailableValues { get; private set; }

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="code"></param>
		/// <param name="longCode"></param>
		/// <param name="description">help text for the option, useful for helper</param>
		/// <param name="values">admitted values for option</param>
		/// <exception cref="System.ArgumentException">Thrown if one of the params is null, empty or whitespace.</exception>
		public LOVOptionDefinitionAttribute(string code, string longCode, string description, params string[] values) : base(code, longCode, description, false)
		{
			if (values == null) throw new ArgumentException(nameof(values));
			if (values.Length == 0) throw new ArgumentException(nameof(values));

			this.AvailableValues = values.ToList();
		}
		/// <summary>
		/// COnstructor to mark the option mandatory
		/// </summary>
		/// <param name="code"></param>
		/// <param name="longCode"></param>
		/// <param name="description">help text for the option, useful for helper</param>
		/// <param name="mandatory">TRUE to make it requested</param>
		/// <param name="values">admitted values for option</param>
		/// <exception cref="System.ArgumentException">Thrown if one of the params is null, empty or whitespace.</exception>
		public LOVOptionDefinitionAttribute(string code, string longCode, string description, bool mandatory, params string[] values) : base(code, longCode, description, mandatory)
		{
			if (values == null) throw new ArgumentException(nameof(values));
			if (values.Length == 0) throw new ArgumentException(nameof(values));

			this.AvailableValues = values.ToList();
		}

	}
}
