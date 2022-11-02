using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CliArgumentParser.ErrorManagement
{
    [Serializable]
    public class WrongOptionUsageException : CliArgumentParserException
    {
        public string Verb { get; private set; }

        public string Option { get; private set; }

        public WrongOptionUsageException() : base()
        { 
            this.Verb = "";
            this.Option  = "";
        }

        protected WrongOptionUsageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Verb = "";
            this.Option = "";
        }

        public WrongOptionUsageException(string verb, Type attributeType) : base($"Wrong Usage: Unmanaged decorator of type {attributeType?.Name} found on Command {verb}")
        {
            this.Verb = verb;
            this.Option = "";
        }

        public WrongOptionUsageException(string verb, string option) : base($"Wrong Usage: invalid option {option} for {verb}")
        {
            this.Verb = verb;
            this.Option = option;
        }

        public WrongOptionUsageException(string verb, string option, string message) : base($"Wrong Usage: invalid option {option} for {verb} - {message}")
        {
            this.Verb = verb;
            this.Option = option;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            base.GetObjectData(info, context);

            info.AddValue("Verb", Verb);
            info.AddValue("Option", Option);
        }
    }
}
