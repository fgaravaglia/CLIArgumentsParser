using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliArgumentParser.ErrorManagement
{
    public class UnknownVerbException : CliArgumentParserException
    {
        public string Verb { get; private set; }

        public UnknownVerbException() : base()
        {
            this.Verb = "";
         }

        

        public UnknownVerbException(string verb) : base($"Wrong Usage: Unkown verb {verb}")
        {
            this.Verb = verb;
        }

    }
}
