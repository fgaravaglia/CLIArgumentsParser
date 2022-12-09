using CliArgumentParser.Decorator;
using CliArgumentParser.ErrorManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CliArgumentParser.Tests.TestCommands
{
    class TestWithListOfValues : CliCommand
    {
        #region Constants

        const string OPT_VALUES = "-values";

        #endregion

        #region Properties

        [OptionList(OPT_VALUES, "Test Values", isMandatory: true)]
        public IEnumerable<string> Values
        {
            get { return this.GetListOfStringArgumentValue<TestWithListOfValues>(x => x.Values); }
            set { this.AddOrUpdateArgument<TestWithListOfValues, IEnumerable<string>>(x => x.Values, value); }
        }

        #endregion

        public TestWithListOfValues() : base("test", "testing a list of values")
        {

        }

        /// <inheritdoc></inheritdoc>
        public override void SetDefaultValues()
        {
            base.SetDefaultValues();

            this.Values = new List<string>();
        }
        /// <inheritdoc></inheritdoc>
        public override void ParseArgument(string[] tokens)
        {
            // validate option
            switch (tokens[0])
            {
                case OPT_VALUES:
                    var value = tokens[1].Split(";").ToList();
                    this.Values = value;
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
                
            };
        }
    }
}
