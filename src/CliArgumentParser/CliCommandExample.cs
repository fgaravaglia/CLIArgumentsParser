using System;
using System.Collections.Generic;
using System.Text;

namespace CliArgumentParser
{
   public class CliCommandExample
    {
        public string Description { get; private set; }

        public string Example { get; private set;  }

        public CliCommandExample(string description, CliCommand cmd)
        {
            if (cmd is null)
                throw new ArgumentNullException(nameof(cmd));

            this.Description = description;
            this.Example = "Example: " + this.Description + Environment.NewLine.ToString() + new CommandStringConverter(cmd).StatementToString();
        }

    }
}
