using System;
using System.Collections.Generic;
using CLIArgumentsParser.Core.Options;

namespace CLIArgumentsParser.Core.Verbs
{
	/// <summary>
	/// Class to map a simple verb
	/// </summary>
	public class Verb
	{
		/// <summary>
		/// char to use to start a verb
		/// </summary>
		internal const string VERB_IDENTIFIER = "-";

		/// <summary>
		/// Name of verb
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Name of verb
		/// </summary>
		public string HelpText { get; private set; }
		/// <summary>
		/// Options of the verb
		/// </summary>
		public List<Option> Options { get; private set; }

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="name">The long name of the verb command.</param>
		/// <param name="helpText">help text to use in helper</param>
		/// <exception cref="System.ArgumentException">Thrown if one of the params  is null, empty or whitespace.</exception>
		private Verb(string name, string helpText)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(helpText));
			this.Name = name;
			this.HelpText = helpText;
			this.Options = new List<Option>();
		}
		/// <summary>
		/// Instances the entity from its definition stored in custom attribute
		/// </summary>
		/// <param name="attribute"></param>
		/// <exception cref="System.ArgumentException">Thrown if one of the params is null</exception>
		internal static Verb FromAttribute(VerbDefinitionAttribute attribute)
		{
			if (attribute == null)
				throw new ArgumentNullException(nameof(attribute));

			var opt = new Verb(attribute.Name, attribute.HelpText);
			return opt;
		}
		/// <summary>
		/// Adds a new option
		/// </summary>
		/// <param name="attribute"></param>
		/// <param name="propertyType"></param>
		internal void AddOptionFromAttribute(OptionDefinitionAttribute attribute, Type propertyType)
		{
			if (propertyType == null)
				throw new ArgumentNullException(nameof(propertyType));
			this.Options.Add(Option.FromAttribute(attribute).OnTargetProperty(propertyType));
		}
	}
}
