using System;
using CLIArgumentsParser.Core;
using CLIArgumentsParser.Core.Usages;
using static CLIArgumentsParser.Tests.Parsing.ParserTestOnMixedArgs;

namespace CLIArgumentsParserTestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var parser = ParserHelper.DefaultParser();
				var options = parser.Parse<TestArguments>(args);

				UsagePrinterHelper.PrintOnConsoleFor<TestArguments>();

				Environment.Exit(0);
			}
			catch (Exception ex)
			{
				Console.WriteLine("*****************************************************************************");
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
				Environment.Exit(1);
			}
		}
	}
}
