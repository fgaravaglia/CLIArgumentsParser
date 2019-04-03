using System;

namespace CLIArgumentsParser.Core.Parsing
{
    /// <summary>
    /// Exception during Parsing arguments
    /// </summary>
    public class InvalidCLIArgumentException : Exception
    {
        /// <summary>
        /// Argument code that throws error
        /// </summary>
        public string ArgumentCode { get; private set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public InvalidCLIArgumentException(string message, string code) : base(message)
        {
            this.ArgumentCode = code;
        }
    }
}
