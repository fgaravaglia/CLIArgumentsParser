using System;
using CLIArgumentsParser.Core;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Parsing;
using CLIArgumentsParser.Core.Verbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLIArgumentsParser.Tests.Parsing
{
	[TestClass]
	public class CLIArgumentAggregatorTest  :BaseUnitTest
	{


		#region Used Types

		public class TestArguments : CLIArguments
		{
			[LOVOptionDefinition("v", "verbosity", "Sets the verbosity of logging", true, new string[] { "DEBUG", "INFO", "WARN", "ERR" })]
			public string Verbosity { get; set; }

			[OptionDefinition("s", "singleOutput", "Writes the output in a single file or not")]
			public bool UseSingleFileAsOutput { get; set; }

			public CopyFilesWith1MandatoryOption CopyWithArguments { get; set; }
		}

		[VerbDefinition("copy", "copy files from SRC to OUTPUT")]
		public class CopyFilesWith1MandatoryOption : CLIArguments
		{
			[OptionDefinition("src", "source", "source folder to use to copy files", mandatory: true)]
			public string SrcFolder { get; set; }
		}

		#endregion

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParseConsoleArguments_WorksFine()
		{
			//******** GIVEN
			string[] arguments = new string[] { "-copy", @"--src='C:\Temp\Pluto'", "--singleOutput", "--v=WARN" };
			var analyizer = new ArgumentModelAnalyzer<TestArguments>();
			analyizer.Analyze();
			var model = analyizer.BuildModel();
			var aggregator = new CLIArgumentAggregator(model);

			//******** WHEN
			string[] parsed = aggregator.AdaptFromCLI(arguments);

			//******** ASSERT
			Assert.IsNotNull(parsed);
			Assert.AreEqual(arguments.Length - 1, parsed.Length);
			Assert.AreEqual(@"-copy --src='C:\Temp\Pluto'", parsed[0]);
			Assert.AreEqual(arguments[3], parsed[1]);
			Assert.AreEqual(arguments[2], parsed[2]);
		}
	}
}
