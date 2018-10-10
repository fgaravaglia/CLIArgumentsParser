using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIArgumentsParser.Core.Verbs
{
	/// <summary>
	/// Class to map a simple verb
	/// </summary>
	public class Verb
	{
		/// <summary>
		/// Name of verb
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Name of verb
		/// </summary>
		public string HelpText { get; private set; }

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

	}
}
