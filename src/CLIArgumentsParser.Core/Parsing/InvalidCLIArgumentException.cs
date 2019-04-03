using System;
using System.Runtime.Serialization;

namespace CLIArgumentsParser.Core.Parsing
{
    /// <summary>
    /// Exception during Parsing arguments
    /// </summary>
    [Serializable]
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
        /// <summary>
        /// Serialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidCLIArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
        /// <summary>
        /// <see cref="Exception.GetObjectData(SerializationInfo, StreamingContext)"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
