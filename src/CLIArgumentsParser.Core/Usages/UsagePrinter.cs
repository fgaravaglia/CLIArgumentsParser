using System;
using System.Linq;
using System.Text;
using CLIArgumentsParser.Core.Options;

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
		/// Default Constructor
		/// </summary>
		protected UsagePrinter(CLIUsageModel model)
		{
			_Model = model ?? throw new ArgumentNullException(nameof(CLIUsageModel));
		}

		/// <summary>
		/// Prints the usage helper text
		/// </summary>
		public void Print()
		{
			PrintLine("Usage:");
			PrintSeparatorLine();
			foreach (var verb in this._Model.Verbs)
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
					if (opt.AvailableValues != null && opt.AvailableValues.Count > 0)
						options.Append($", Admitted Values: {String.Join(";", opt.AvailableValues)}");
					PrintLine($"\t\t{options.ToString()}");
				}

				// check usages
				if (verb.Examples.Count() > 0)
				{
					PrintLine($"\tExample:");
					verb.Examples.ToList().ForEach(x =>
					{
						PrintLine($"\t#{verb.Examples.ToList().IndexOf(x)}  {x.HelpText}: {x.Sample.ToCLIString()}");
					});
					PrintLine(" ");
				}
			}

			foreach (var opt in this._Model.Options)
			{
				PrintLine($"\t{Option.OPTION_IDENTIFIER}{opt.Code} ({Option.OPTION_IDENTIFIER}{opt.LongCode}) {opt.HelpText}");
				// check usages
				if (opt.Examples.Count() > 0)
				{
					PrintLine($"\tExample:");
					opt.Examples.ToList().ForEach(x =>
					{
						PrintLine($"\t#{opt.Examples.ToList().IndexOf(x)}  {x.HelpText}: {x.Sample.ToCLIString()}");
					});
					PrintLine(" ");
				}
			}

			if (this._Model.Examples.Count() > 0)
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
	}
}
