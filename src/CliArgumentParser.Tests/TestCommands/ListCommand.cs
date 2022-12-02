using CliArgumentParser.Decorator;
using CliArgumentParser.ErrorManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliArgumentParser.Tests.TestCommands
{
    class ListCommand : CliCommand
    {
        #region Constants

        const string OPT_VERBOSE = "-verbose";
        const string OPT_FOLDER = "-folder";
        const string OPT_PCK_FILTER_NAME = "-pckName";

        #endregion

        #region Properties

        [Option(OPT_FOLDER, "Folder to List", isMandatory: true)]
        public string Folder
        {
            get { return this.GetArgumentValue<ListCommand, string>(x => x.Folder); }
            set { this.AddOrUpdateArgument<ListCommand, string>(x => x.Folder, value); }
        }

        [Option(OPT_PCK_FILTER_NAME, "Filter on Package Name", isMandatory: false)]
        public string PackageNameFilter
        {
            get { return this.GetArgumentValue<ListCommand, string>(x => x.PackageNameFilter); }
            set { this.AddOrUpdateArgument<ListCommand, string>(x => x.PackageNameFilter, value); }
        }

        [Option(OPT_VERBOSE, "Verbosity Level", isMandatory: false)]
        public bool IsVerbose
        {
            get { return this.GetBooleanArgumentValue<ListCommand, bool>(x => x.IsVerbose); }
            set { this.AddOrUpdateArgument<ListCommand, bool>(x => x.IsVerbose, value); }
        }

        #endregion

        public ListCommand() : base("list", "List Packages inside folder")
        {

        }

        /// <inheritdoc></inheritdoc>
        public override void SetDefaultValues()
        {
            base.SetDefaultValues();

            this.IsVerbose = false;
            this.Folder = @"C:\Temp";
            this.PackageNameFilter = "";
        }
        /// <inheritdoc></inheritdoc>
        public override void ParseArgument(string[] tokens)
        {
            // validate option
            switch (tokens[0])
            {
                case OPT_FOLDER:
                    this.Folder = tokens[1];
                    break;
                case OPT_PCK_FILTER_NAME:
                    this.PackageNameFilter = tokens[1];
                    break;

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
            var cmd1 = new ListCommand();
            cmd1.SetDefaultValues();
            cmd1.Folder = @"C:\Temp";
            return new List<CliCommandExample>()
            {
                new CliCommandExample("List with no filter", cmd1),
            };
        }
    }
}
