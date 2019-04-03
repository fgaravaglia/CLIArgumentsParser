using System;

namespace CLIArgumentsParser.Core.Verbs
{
    /// <summary>
    /// Models a verb command specification.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class VerbDefinitionAttribute : Attribute
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
        public VerbDefinitionAttribute(string name, string helpText)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (string.IsNullOrWhiteSpace(helpText)) throw new ArgumentException(nameof(helpText));
            this.Name = name;
            this.HelpText = helpText;
        }
    }
}
