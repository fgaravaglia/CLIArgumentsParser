using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliArgumentParser.ErrorManagement
{
    public class WrongOptionUsageException : CliArgumentParserException
    {
        public string Verb { get; private set; }

        public string Option { get; private set; }

        public WrongOptionUsageException() : base()
        { 
            this.Verb = "";
            this.Option  = "";
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
    }
}
