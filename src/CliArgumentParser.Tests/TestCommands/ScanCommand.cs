using CliArgumentParser.Decorator;
using CliArgumentParser.ErrorManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliArgumentParser.Tests.TestCommands
{
    class ScanCommand : CliCommand
    {
        #region Constants

        const string OPT_FOLDER = "-folder";
        const string OPT_FILENAME_SEARCH_PATTERN = "-match-file";
        const string OPT_FILECONTENT_SEARCH_PATTERN = "-match-exp";
        const string OPT_TO = "-to";

        #endregion

        #region Properties

        [Option(OPT_FOLDER, "Root folder to scan", isMandatory: true)]
        public string Folder
        {
            get { return this.GetArgumentValue<ScanCommand, string>(x => x.Folder); }
            set { this.AddOrUpdateArgument<ScanCommand, string>(x => x.Folder, value); }
        }

        [Option(OPT_FILENAME_SEARCH_PATTERN, "Expression to filter scan result", isMandatory: false)]
        public string FileMatchExpression
        {
            get { return this.GetArgumentValue<ScanCommand, string>(x => x.FileMatchExpression); }
            set { this.AddOrUpdateArgument<ScanCommand, string>(x => x.FileMatchExpression, value); }
        }

        [Option(OPT_FILECONTENT_SEARCH_PATTERN, "Expression to search into scanned files", isMandatory: false)]
        public string ContentMatchExpression
        {
            get { return this.GetArgumentValue<ScanCommand, string>(x => x.ContentMatchExpression); }
            set { this.AddOrUpdateArgument<ScanCommand, string>(x => x.ContentMatchExpression, value); }
        }

        [Option(OPT_TO, "Persistence Driver for result", isMandatory: false, domain: new string[] { "", "CSV"})]
        public string PersistedTo
        {
            get { return this.GetArgumentValue<ScanCommand, string>(x => x.PersistedTo); }
            set { this.AddOrUpdateArgument<ScanCommand, string>(x => x.PersistedTo, value); }
        }

        public bool IsPersistedToCSV
        {
            get
            {
                var persisted = this.PersistedTo;
                return !string.IsNullOrEmpty(persisted) && persisted.ToUpperInvariant() == "CSV";
            }
        }

        #endregion

        public ScanCommand() : base("scan", "Scan the target folder tree")
        {

        }

        public static ScanCommand AsExampleFor(string folder, string matchExpr, string contentMatchExpr, string persistence)
        {
            var cmd = new ScanCommand();

            cmd.Folder = folder;
            cmd.FileMatchExpression = matchExpr;
            cmd.ContentMatchExpression = contentMatchExpr;
            cmd.PersistedTo = persistence;

            return cmd;
        }
        /// <inheritdoc></inheritdoc>
        public override void SetDefaultValues()
        {
            base.SetDefaultValues();

            this.Folder = "";
            this.FileMatchExpression = "";
            this.ContentMatchExpression = "";
            this.PersistedTo = "";
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
                    case OPT_FOLDER:
                    case OPT_FILENAME_SEARCH_PATTERN:
                    case OPT_FILECONTENT_SEARCH_PATTERN:
                        this.UpdateArgumentValue(tokens[0], tokens[1]);
                        break;

                    case OPT_TO:
                        this.PersistedTo = tokens[1].Trim().ToUpperInvariant();
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
                new CliCommandExample("Scan the target folder to search *.csproj Files, containing the text \"NugetPackages\" and save a CSV files with output",
                                        ScanCommand.AsExampleFor(@"C:\Temp\MyFolder", ".csproj", @"\NugetPackages\", "CSV"))
            };
        }
    }
}
