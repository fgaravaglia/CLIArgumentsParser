using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CliArgumentParser.ErrorManagement
{
    [Serializable]
    public class CliArgumentParserException : Exception
    {
        public CliArgumentParserException() : base()
        { }

        public CliArgumentParserException(string message) : base(message)
        {
        }

        protected CliArgumentParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            base.GetObjectData(info, context);
        }
    }
}
