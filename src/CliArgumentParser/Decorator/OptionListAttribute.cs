using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CliArgumentParser.Decorator
{
    /// <summary>
    /// Decorator to set up an option that represents a list of strings
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionListAttribute : OptionAttribute
    {
        /// <summary>
        /// seprator for list values
        /// </summary>
        /// <value></value>
        public string ListValueSeparator {get; private set;}

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="descr"></param>
        /// <param name="isMandatory"></param>
        /// <param name="values"></param>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        public OptionListAttribute(string name, string descr, bool isMandatory, string separator = ";") 
            : base(name, descr, isMandatory, Array.Empty<string>())
        {
            if(String.IsNullOrEmpty(separator))
            throw new ArgumentNullException(nameof(separator));

            this.ListValueSeparator = separator;
        }
    }
}