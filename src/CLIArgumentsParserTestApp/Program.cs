using System;
using CLIArgumentsParser.Core;
using static CLIArgumentsParser.Tests.Parsing.ParserTestOnMixedArgs;

namespace CLIArgumentsParserTestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var options = ParserHelper.DefaultParser().Parse<TestArguments>(args);

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
