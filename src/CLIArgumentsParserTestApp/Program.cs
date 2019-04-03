using CLIArgumentsParser.Core;
using CLIArgumentsParser.Core.Usages;
using System;
using static CLIArgumentsParser.Tests.Parsing.ParserTestOnMixedArgs;

namespace CLIArgumentsParserTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // configure parser
                var info = new CLIInfo();
                var parser = ParserHelper.DefaultParser()
                                .UsingThisVersionInOutput(info, Console.WriteLine)
                                .PrintingHelperOnErrorAndQuit<TestArguments>(info, Console.WriteLine);
                // try to parse arguments; if thorws errors, the program quits
                var options = parser.Parse<TestArguments>(args);
                Console.WriteLine(options != null ? "Options succesfully parsed" : "Options is currently null");
                UsagePrinterHelper.PrintOnConsoleFor<TestArguments>(info.Alias);

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
