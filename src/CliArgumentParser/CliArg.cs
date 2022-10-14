using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliArgumentParser
{
   
    public class CliArg
    {
        public string Name { get; private set; }

        public string Value { get; private set; }

        public string Description { get; private set; }

        public CliArg(string name, string value, string description)
        {
            this.Name = name;
            this.Value = value;
            this.Description = description;
        }

        public void UpdateValue(string value)
        {
            this.Value = value;
        }

        public void UpdateDescription(string description)
        {
            this.Description = description;
        }

    }
}
