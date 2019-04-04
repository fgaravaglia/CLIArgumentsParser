using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Verbs;
using System;
using System.Linq;
using System.Text;

namespace CLIArgumentsParser.Core.Usages
{
    /// <summary>
    /// Abstract component to print the usage of CLI for a given Model
    /// </summary>
    public abstract class UsagePrinter
    {
        /// <summary>
        /// Cli usage
        /// </summary>
        readonly protected CLIUsageModel _Model;
        /// <summary>
        /// Alias to call cli
        /// </summary>
        readonly protected string _Alias;

        /// <summary>
        /// Default Constructor
        /// </summary>
        protected UsagePrinter(CLIUsageModel model, string alias)
        {
            _Model = model ?? throw new ArgumentNullException(nameof(CLIUsageModel));
            _Alias = alias ?? throw new ArgumentNullException(nameof(alias));
        }

        /// <summary>
        /// Prints the usage helper text
        /// </summary>
        public void Print()
        {
            PrintSeparatorLine();
            PrintLine($"Helper for {_Alias}");
            PrintLine($"Usage:");
            PrintSeparatorLine();

            foreach (var verb in this._Model.Verbs)
            {
                PrintVerb(verb);
            }

            foreach (var opt in this._Model.Options)
            {
                PrintOption(opt);
            }

            if (this._Model.Examples.Any())
            {
                PrintLine(" ");
                PrintLine("--------------------| Examples |------------------");
                this._Model.Examples.ToList().ForEach(x =>
                {
                    PrintLine($"\t#{this._Model.Examples.ToList().IndexOf(x)}  {x.HelpText}: {x.Sample.ToCLIString()}");
                });
                PrintLine(" ");
            }

        }

        /// <summary>
        /// Print on output the input line
        /// </summary>
        /// <param name="line"></param>
        protected abstract void PrintLine(string line);

        private void PrintSeparatorLine()
        {
            PrintLine("-------------------------------------------------------------------------------------------");
        }

        private void PrintVerb(Verb verb)
        {
            PrintLine($"\t{verb.Code}\t{verb.HelpText}");
            PrintLine("\tOptions:");
            foreach (var opt in verb.Options)
            {
                PrintLine($"\t\t{Option.OPTION_IDENTIFIER}{opt.Code} ({Option.OPTION_IDENTIFIER}{opt.LongCode}) {opt.HelpText}");
                StringBuilder options = new StringBuilder();
                options.Append($"Mandatory: {opt.Mandatory}");
                if (opt.DefaultValue != null)
                    options.Append($", Default: {opt.DefaultValue.ToString()}");

                if (opt.AvailableValues != null && opt.AvailableValues.Any())
                    options.Append($", Admitted Values: {String.Join(";", opt.AvailableValues)}");

                PrintLine($"\t\t{options.ToString()}");
            }

            // check usages
            if (verb.Examples.Any())
            {
                PrintLine($"\tExample:");
                verb.Examples.ToList().ForEach(x =>
                {
                    PrintLine($"\t#{verb.Examples.ToList().IndexOf(x)}  {x.HelpText}: {x.Sample.ToCLIString()}");
                });
                PrintLine(" ");
            }
        }

        private void PrintOption(Option opt)
        {
            PrintLine($"\t{Option.OPTION_IDENTIFIER}{opt.Code} ({Option.OPTION_IDENTIFIER}{opt.LongCode}) {opt.HelpText}");
            // check usages
            if (opt.Examples.Any())
            {
                PrintLine($"\tExample:");
                opt.Examples.ToList().ForEach(x =>
                {
                    PrintLine($"\t#{opt.Examples.ToList().IndexOf(x)}  {x.HelpText}: {x.Sample.ToCLIString()}");
                });
                PrintLine(" ");
            }
        }
    }
}
