using System;

namespace CLIArgumentsParser.Core.Options
{
	/// <summary>
	/// Attribute to decorate a property as a CLI Option
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class OptionDefinitionAttribute : Attribute
	{
		/// <summary>
		/// Code of the option, without option initializer char
		/// </summary>
		/// <example>o, src, i</example>
		public string Code { get; private set; }
		/// <summary>
		/// Long Code of the option, without option initializer char
		/// </summary>
		/// <example>output, source, input</example>
		public string LongCode { get; private set; }
		/// <summary>
		/// Description of the option, like helper text
		/// </summary>
		public string Description { get; private set; }
		/// <summary>
		/// TRUE if the option is mandatory
		/// </summary>
		public bool Mandatory { get; private set; }
		/// <summary>
		/// Default value to apply to option
		/// </summary>
		public object DefaultValue { get; private set; }

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="code"></param>
		/// <param name="longCode"></param>
		/// <param name="description">help text for the option, useful for helper</param>
		/// <exception cref="System.ArgumentException">Thrown if one of the params is null, empty or whitespace.</exception>
		public OptionDefinitionAttribute(string code, string longCode, string description) : base()
		{
			if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException(nameof(code));
			if (string.IsNullOrWhiteSpace(longCode)) throw new ArgumentException(nameof(longCode));
			if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException(nameof(description));

			this.Code = code;
			this.LongCode = longCode;
			this.Description = description;
			this.Mandatory = false;
			this.DefaultValue = null;
		}
		/// <summary>
		/// COnstructor to mark the option mandatory
		/// </summary>
		/// <param name="code"></param>
		/// <param name="longCode"></param>
		/// <param name="description">help text for the option, useful for helper</param>
		/// <param name="mandatory">TRUE to make it requested</param>
		/// <exception cref="System.ArgumentException">Thrown if one of the params is null, empty or whitespace.</exception>
		public OptionDefinitionAttribute(string code, string longCode, string description, bool mandatory) : this(code, longCode, description)
		{
			this.Mandatory = mandatory;
		}
		/// <summary>
		/// COnstructor to mark the option mandatory and with a default value
		/// </summary>
		/// <param name="code"></param>
		/// <param name="longCode"></param>
		/// <param name="description">help text for the option, useful for helper</param>
		/// <param name="mandatory">TRUE to make it requested</param>
		/// <param name="defaultValue"></param>
		/// <exception cref="System.ArgumentException">Thrown if one of the params is null, empty or whitespace.</exception>
		public OptionDefinitionAttribute(string code, string longCode, string description, bool mandatory, object defaultValue) : this(code, longCode, description)
		{
			this.Mandatory = mandatory;
			this.DefaultValue = defaultValue;
		}
	}
}
