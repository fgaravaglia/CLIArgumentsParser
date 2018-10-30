using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLIArgumentsParser.Core;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Parsing;
using CLIArgumentsParser.Core.Usages;
using CLIArgumentsParser.Core.Verbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLIArgumentsParser.Tests.Usages
{
	[TestClass]
	public class StringBuilderUsagePrinterTest : BaseUnitTest
	{
		StringBuilderUsagePrinter _Printer;

		[TestMethod]
		public void ExampleOnVerbAreSuccesfullyPrinted()
		{
			//******** GIVEN
			var testArguments = new OptionsPlusVerbArguments() { CopyWithArguments = new CopyFilesWith1MandatoryOption() };
			var analyzer = new ArgumentModelAnalyzer<OptionsPlusVerbArguments>();
			analyzer.Analyze();
			var model = analyzer.BuildModel();
			this._Printer = new StringBuilderUsagePrinter(model, "my.exe");

			//******** WHEN
			this._Printer.Print();

			//******** ASSERT
			var text = this._Printer.GetText();
			Assert.IsNotNull(text);
			Assert.IsTrue(text.Contains(testArguments.CopyWithArguments.Examples.ToList()[0].HelpText), $"Unable to find help text {testArguments.CopyWithArguments.Examples.ToList()[0].HelpText}");
		}

		[TestMethod]
		public void ExampleOnRootArgumentIsSuccesfullyPrinted()
		{
			//******** GIVEN
			var testArguments = new OnlyOptionsArguments();
			var analyzer = new ArgumentModelAnalyzer<OnlyOptionsArguments>();
			analyzer.Analyze();
			var model = analyzer.BuildModel();
			this._Printer = new StringBuilderUsagePrinter(model, "my.exe");

			//******** WHEN
			this._Printer.Print();

			//******** ASSERT
			var text = this._Printer.GetText();
			Assert.IsNotNull(text);
			Assert.IsTrue(text.Contains(testArguments.Examples.ToList()[0].HelpText), $"Unable to find help text {testArguments.Examples.ToList()[0].HelpText}");
			Assert.IsTrue(text.Contains(testArguments.Examples.ToList()[1].HelpText), $"Unable to find help text {testArguments.Examples.ToList()[1].HelpText}");
		}

		#region Used Types

		public class OnlyOptionsArguments : CLIArguments
		{
			[LOVOptionDefinition("v", "verbosity", "Sets the verbosity of logging", true, new string[] { "DEBUG", "INFO", "WARN", "ERR" })]
			public string Verbosity { get; set; }

			[OptionDefinition("s", "singleOutput", "Writes the output in a single file or not")]
			public bool UseSingleFileAsOutput { get; set; }

			public override string ToCLIString()
			{
				StringBuilder builder = new StringBuilder();

				builder.Append($" --v={Verbosity}");
				if (UseSingleFileAsOutput)
					builder.Append($" --s");

				return builder.ToString();
			}

			protected override IEnumerable<CLIUsageExample> BuildExamples()
			{
				var list = new List<CLIUsageExample>();
				list.Add(new CLIUsageExample("Set custom verbsity", new OnlyOptionsArguments() { Verbosity = "WARN" }));
				list.Add(new CLIUsageExample("Save output in a single file", new OnlyOptionsArguments() { UseSingleFileAsOutput = true }));
				return list;
			}
		}

		public class OptionsPlusVerbArguments : CLIArguments
		{
			[LOVOptionDefinition("v", "verbosity", "Sets the verbosity of logging", true, new string[] { "DEBUG", "INFO", "WARN", "ERR" })]
			public string Verbosity { get; set; }

			[OptionDefinition("s", "singleOutput", "Writes the output in a single file or not")]
			public bool UseSingleFileAsOutput { get; set; }

			public CopyFilesWith1MandatoryOption CopyWithArguments { get; set; }

			public override string ToCLIString()
			{
				StringBuilder builder = new StringBuilder();

				builder.Append(this.CopyWithArguments.ToCLIString());
				builder.Append($" --v={Verbosity}");
				if (UseSingleFileAsOutput)
					builder.Append($" --s");

				return builder.ToString();
			}
		}

		[VerbDefinition("copy", "copy files from SRC to OUTPUT")]
		public class CopyFilesWith1MandatoryOption : CLIArguments
		{
			[OptionDefinition("src", "source", "source folder to use to copy files", mandatory: true)]
			public string SrcFolder { get; set; }

			public override string ToCLIString()
			{
				StringBuilder builder = new StringBuilder();

				builder.Append($" --src={SrcFolder}");

				return builder.ToString();
			}

			protected override IEnumerable<CLIUsageExample> BuildExamples()
			{
				var list = new List<CLIUsageExample>();
				list.Add(new CLIUsageExample("Copy from target folder", new CopyFilesWith1MandatoryOption() { SrcFolder = @"'C:\Temp\Test'" }));
				return list;
			}
		}

		#endregion
	}
}
