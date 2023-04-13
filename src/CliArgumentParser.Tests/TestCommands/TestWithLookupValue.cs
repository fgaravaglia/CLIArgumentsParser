using CliArgumentParser.Decorator;
using CliArgumentParser.ErrorManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliArgumentParser.Tests.TestCommands
{
    class TestWithLookupValue : CliCommand
    {
        #region Constants

        const string OPT_DOMAIN = "-domain";

        #endregion

        #region Properties

        [Option(OPT_DOMAIN, "Domain", isMandatory: true, domain: new string[]{"VAL1", "VAL2", "VAL3"})]
        public string Domain
        {
            get { return this.GetArgumentValue<TestWithLookupValue, string>(x => x.Domain); }
            set { this.AddOrUpdateArgument<TestWithLookupValue, string>(x => x.Domain, value); }
        }

        #endregion

        public TestWithLookupValue() : base("testWithDomainValue", "testing a lookup value")
        {

        }

        /// <inheritdoc></inheritdoc>
        public override void SetDefaultValues()
        {
            base.SetDefaultValues();

            this.Domain = "";
        }
        /// <inheritdoc></inheritdoc>
        public override void ParseArgument(string[] tokens)
        {
            // validate option
            switch (tokens[0])
            {
                case OPT_DOMAIN:
                    this.Domain = tokens[1].Trim().ToUpperInvariant(); 
                    break;

                default:
                    throw new WrongOptionUsageException(this.Verb, tokens[0]);
            }
        }
        /// <inheritdoc></inheritdoc>
        public override List<CliCommandExample> Examples()
        {
            return new List<CliCommandExample>()
            {
                new CliCommandExample("Test the domain", new TestWithLookupValue()),
            };
        }
    }
}
