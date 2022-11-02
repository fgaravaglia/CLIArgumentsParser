using System;
using System.Collections.Generic;
using System.Text;

namespace CliArgumentParser.ErrorManagement
{
    public class CliArgumentParserException : Exception
    {
        public CliArgumentParserException() : base()
        { }

        public CliArgumentParserException(string message) : base(message)
        {
        }

    }
}
