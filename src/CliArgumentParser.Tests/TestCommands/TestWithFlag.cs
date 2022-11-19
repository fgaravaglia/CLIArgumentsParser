using CliArgumentParser.Decorator;
using CliArgumentParser.ErrorManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliArgumentParser.Tests.TestCommands
{
    class TestWithFlagCommand : CliCommand
    {
        #region Constants

        const string OPT_VERBOSE = "-verbose";

        #endregion

        #region Properties

        [Option(OPT_VERBOSE, "Verbosity Level", isMandatory: true)]
        public bool IsVerbose
        {
            get { return this.GetBooleanArgumentValue<TestWithFlagCommand, bool>(x => x.IsVerbose); }
            set { this.AddOrUpdateArgument<TestWithFlagCommand, bool>(x => x.IsVerbose, value); }
        }

        #endregion

        public TestWithFlagCommand() : base("flag", "testing a flag")
        {

        }

        /// <inheritdoc></inheritdoc>
        public override void SetDefaultValues()
        {
            base.SetDefaultValues();

            this.IsVerbose = false;
        }
        /// <inheritdoc></inheritdoc>
        public override void ParseArgument(string[] tokens)
        {
            // validate option
            switch (tokens[0])
            {
                case OPT_VERBOSE:
                    this.IsVerbose = true;
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
                new CliCommandExample("Test the flag false", new TestWithFlagCommand()),
            };
        }
    }
}
