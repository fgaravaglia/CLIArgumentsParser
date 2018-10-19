using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIArgumentsParser.Core.Usages
{
	/// <summary>
	/// COmponent to print usage of CLI on console output
	/// </summary>
	public class ConsoleUsagePrinter : UsagePrinter
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ConsoleUsagePrinter(CLIUsageModel model) : base(model)
		{ 
		
		}

		/// <summary>
		/// print the line in console ouput
		/// </summary>
		/// <param name="line"></param>
		protected override void PrintLine(string line)
		{
			Console.WriteLine(line);
		}
	}
}
