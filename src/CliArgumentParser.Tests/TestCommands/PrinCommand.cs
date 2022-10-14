using CliArgumentParser.Decorator;
using CliArgumentParser.ErrorManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliArgumentParser.Tests.TestCommands
{
    class PrintCommand : CliCommand
    {
        #region Constants

        const string OPT_VERBOSE = "-verbosity";

        #endregion

        #region Properties

        [Option(OPT_VERBOSE, "Verbosity Level", isMandatory: true, domain: new string[] { "DEBUG", "INFO" })]
        public string Verbosity
        {
            get { return this.GetArgumentValue<PrintCommand, string>(x => x.Verbosity); }
            set { this.AddOrUpdateArgument<PrintCommand, string>(x => x.Verbosity, value); }
        }

        #endregion

        public PrintCommand() : base("print", "Print a message")
        {

        }

        /// <inheritdoc></inheritdoc>
        public override void SetDefaultValues()
        {
            base.SetDefaultValues();

            this.Verbosity = "DEBUG";
        }
        /// <inheritdoc></inheritdoc>
        public override void ParseArgument(string[] tokens)
        {
            // take the expecetd value
            if (tokens.Length == 1)
            {
                throw new NotImplementedException();
            }
            else
            {
                // validate option
                switch (tokens[0])
                {
                    case OPT_VERBOSE:
                        this.Verbosity = tokens[1].Trim().ToUpperInvariant();
                        break;

                    default:
                        throw new WrongOptionUsageException(this.Verb, tokens[0]);
                }
            }
        }
        /// <inheritdoc></inheritdoc>
        public override List<CliCommandExample> Examples()
        {
            return new List<CliCommandExample>()
            {
                new CliCommandExample("Print message", new PrintCommand())
            };
        }
    }
}
