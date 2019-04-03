using CLIArgumentsParser.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CLIArgumentsParser.Core.Verbs
{
    /// <summary>
    /// Class to map a simple verb
    /// </summary>
    public class Verb : ICLIArgumentModel
    {
        /// <summary>
        /// char to use to start a verb
        /// </summary>
        internal const string VERB_IDENTIFIER = "/";

        /// <summary>
        /// Code to identify the verb
        /// </summary>
        public string Code { get { return Name; } }
        /// <summary>
        /// Name of verb
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Description of the Verb, like helper text
        /// </summary>
        public string HelpText { get; private set; }
        /// <summary>
        /// Gets the type of argument (Verb, Option)
        /// </summary>
        public string ArgumentType { get { return "VERB"; } }
        /// <summary>
        /// Gets the list of examples for the usages
        /// </summary>
        public IEnumerable<CLIUsageExample> Examples { get; private set; }
        /// <summary>
        /// Type of option value
        /// </summary>
        public Type TargetType { get; private set; }
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
            if (string.IsNullOrWhiteSpace(helpText)) throw new ArgumentException(nameof(helpText));
            this.Name = name;
            this.HelpText = helpText;
            this.Options = new List<Option>();
            this.Examples = new List<CLIUsageExample>();
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
        /// sets the property type marked by this option
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal Verb OnTargetProperty(Type target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            this.TargetType = target;
            return this;
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
        /// <summary>
        /// Adds the example
        /// </summary>
        /// <param name="example"></param>
        /// <returns></returns>
        internal Verb AddExample(CLIUsageExample example)
        {
            if (example == null)
                throw new ArgumentNullException(nameof(example));

            var examples = this.Examples.ToList();
            examples.Add(example);
            this.Examples = examples;
            return this;
        }
    }
}
