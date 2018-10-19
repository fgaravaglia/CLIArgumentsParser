using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIArgumentsParser.Core
{
	/// <summary>
	/// Example of usage for the CLI
	/// </summary>
	public class CLIUsageExample
	{
		/// <summary>
		/// Help text to print to user
		/// </summary>
		public string HelpText { get; private set; }
		/// <summary>
		/// CLI sample
		/// </summary>
		public ICLIArguments Sample { get; private set; }

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="text"></param>
		/// <param name="sample"></param>
		public CLIUsageExample(String text, ICLIArguments sample)
		{
			this.HelpText = text;
			this.Sample = sample;
		}
	}
}
