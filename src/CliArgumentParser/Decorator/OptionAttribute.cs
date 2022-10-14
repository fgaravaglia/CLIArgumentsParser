using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CliArgumentParser.Decorator
{
    [Serializable]
    public class OptionAttribute : Attribute
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        public bool IsMandatory { get; private set; }

        public string UsageNotes { get; private set; }

        public List<string> ValidValues { get; private set; }

        public OptionAttribute(string name, string descr, bool isMandatory) : this(name, descr, isMandatory, new string[] { })
        {
            this.Name = name;
            this.Description = descr;
            this.IsMandatory = isMandatory;
            this.ValidValues = new List<string>();
        }

        public OptionAttribute(string name, string descr, bool isMandatory, string[] domain, string usageNotes = "") : base()
        {
            this.Name = name;
            this.Description = descr;
            this.IsMandatory = isMandatory;
            this.UsageNotes = usageNotes;
            this.ValidValues = domain != null ? domain.ToList() : new List<string>();
            if (domain != null && domain.Any())
            {
                var domainStr = String.Join(";", domain.Select(x => "'" + x + "'"));
                this.UsageNotes = String.IsNullOrEmpty(usageNotes) ? $"Valid Values: [{domainStr}]" : $"{usageNotes}; Valid Values: [{domainStr}]";
            }
        }
    }
}
