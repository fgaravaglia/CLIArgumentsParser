using CLIArgumentsParser.Core.Parsing;

namespace CLIArgumentsParser.Core.Usages
{
	/// <summary>
	/// Helper to use the Usage printer classes
	/// </summary>
	public static class UsagePrinterHelper
	{
		/// <summary>
		/// Prints the usage for given type <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">class that mape arguments for CLI</typeparam>
		public static void PrintOnConsoleFor<T>(string alias) where T : ICLIArguments
		{
			// analyze the target type and build the model for usage
			var analyzer = new ArgumentModelAnalyzer<T>();
			analyzer.Analyze();
			var model = analyzer.BuildModel();

			// print usage
			var helper = new ConsoleUsagePrinter(model, alias);
			helper.Print();
		}
	}
}
