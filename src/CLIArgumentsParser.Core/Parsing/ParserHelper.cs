using System;
using CLIArgumentsParser.Core.Usages;

namespace CLIArgumentsParser.Core
{
	/// <summary>
	/// Extension class for parser
	/// </summary>
	public static class ParserHelper
	{
		/// <summary>
		/// Instances a new parser
		/// </summary>
		/// <returns></returns>
		public static Parser DefaultParser()
		{
			Parser parser = new Parser();
			return parser;
		}
		/// <summary>
		/// Set the error callback to print also the helper
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="p"></param>
		/// <param name="cliInfo"></param>
		/// <param name="output"></param>
		/// <returns></returns>
		public static Parser PrintingHelperOnErrorAndQuit<T>(this Parser p, ICLIInfo cliInfo, Action<string> output) where T : ICLIArguments
		{
			if (p == null)
				throw new ArgumentNullException(nameof(p));
			if (cliInfo == null)
				throw new ArgumentNullException(nameof(cliInfo));
			if (String.IsNullOrEmpty(cliInfo.Alias))
				throw new ArgumentNullException(nameof(cliInfo));
			if (output == null)
				throw new ArgumentNullException(nameof(output));

			p.OnError(err =>
			 {
				 output("**********************************| Unexpected Error during argument parsing |***************************************");
				 output(err.Message);
				 output(err.StackTrace);
				 output("");
				 output(" Please, run exe without params to see the helper");
				 UsagePrinterHelper.PrintOnConsoleFor<T>(cliInfo.Alias);
				 Environment.Exit(-2);
			 });
			return p;
		}
		/// <summary>
		/// Set the error callback to print also the helper
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="p"></param>
		/// <param name="cliInfo"></param>
		/// <returns></returns>
		public static Parser PrintingHelperOnErrorAndQuit<T>(this Parser p, ICLIInfo cliInfo) where T : ICLIArguments
		{
			return PrintingHelperOnErrorAndQuit<T>(p, cliInfo, Console.WriteLine);
		}
		/// <summary>
		/// Set proper output
		/// </summary>
		/// <param name="p"></param>
		/// <param name="cliInfo"></param>
		/// <param name="output"></param>
		/// <returns></returns>
		public static Parser UsingThisVersionInOutput(this Parser p, ICLIInfo cliInfo, Action<string> output)
		{
			if (p == null)
				throw new ArgumentNullException(nameof(p));
			if (cliInfo == null)
				throw new ArgumentNullException(nameof(cliInfo));
			if (String.IsNullOrEmpty(cliInfo.Alias))
				throw new ArgumentNullException(nameof(cliInfo));
			if (output == null)
				throw new ArgumentNullException(nameof(output));

			p.OnSuccess(arg =>
			{
				output("");
				var line = $" {cliInfo.Alias} - Version {cliInfo.Version.MajorNumber}.{cliInfo.Version.MinorNumber}"
							+ $" (released at {cliInfo.Version.ReleasedOn.Month.ToString("00")}/{cliInfo.Version.ReleasedOn.Year})";
				output(line);
				if (DateTime.Today.Year == cliInfo.Version.ReleasedOn.Year)
					output($" (C) {cliInfo.Version.ReleasedOn.Year} - All right reserved");
				else
					output($" (C) {DateTime.Today.Year } - {cliInfo.Version.ReleasedOn.Year} - All right reserved");
				output(" Arguments parsed succesfully");
			});
			return p;
		}
		/// <summary>
		/// Set proper output
		/// </summary>
		/// <param name="p"></param>
		/// <param name="cliInfo"></param>
		/// <returns></returns>
		public static Parser UsingThisVersionInOutput(this Parser p, ICLIInfo cliInfo)
		{
			return p.UsingThisVersionInOutput(cliInfo, Console.WriteLine);
		}
	}
}
