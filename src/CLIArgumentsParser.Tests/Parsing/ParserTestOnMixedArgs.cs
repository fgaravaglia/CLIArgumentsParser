using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLIArgumentsParser.Core;
using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Verbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLIArgumentsParser.Tests.Parsing
{
	[TestClass]
	public class ParserTestOnMixedArgs : BaseUnitTest
	{
		Parser _Parser;

		protected override void InitializeMyTestData()
		{
			base.InitializeMyTestData();

			this._Parser = new Parser();
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParseVerbWithOptionsAnd2Options_WorksFine()
		{
			//******** GIVEN
			string[] arguments = new string[] { @"/copy --src='C:\Temp\Pluto'", "--singleOutput" , "--v=WARN"};
			Exception thrownEx = null;

			//******** WHEN
			TestArguments parsed = this._Parser.UseAggregatedArguments().Parse<TestArguments>(arguments);

			//******** ASSERT
			thrownEx = this._Parser.GetLastError();
			string message = thrownEx == null ? "" : thrownEx.Message;
			Assert.IsNull(thrownEx, message);
			Assert.IsTrue(parsed.UseSingleFileAsOutput, "Wrong value for UseSingleFileAsOutput");
			Assert.IsFalse(String.IsNullOrEmpty(parsed.Verbosity), "Wrong value for Verbosity: cannot be null");
			Assert.AreEqual("WARN", parsed.Verbosity, "Wrong value for Verbosity");
			Assert.IsNotNull(parsed.CopyWithArguments);
			Assert.AreEqual(@"'C:\Temp\Pluto'", parsed.CopyWithArguments.SrcFolder, "Wrong value for CopyWithArguments.SrcFolder");
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParseConsoleArguments_WorksFine()
		{
			//******** GIVEN
			string[] arguments = new string[] { "/copy", @"--src='C:\Temp\Pluto'", "--singleOutput", "--v=WARN" };
			Exception thrownEx = null;

			//******** WHEN
			TestArguments parsed = ParserHelper.DefaultParser().Parse<TestArguments>(arguments);

			//******** ASSERT
			thrownEx = this._Parser.GetLastError();
			string message = thrownEx == null ? "" : thrownEx.Message;
			Assert.IsNull(thrownEx, message);
			Assert.IsTrue(parsed.UseSingleFileAsOutput, "Wrong value for UseSingleFileAsOutput");
			Assert.IsFalse(String.IsNullOrEmpty(parsed.Verbosity), "Wrong value for Verbosity: cannot be null");
			Assert.AreEqual("WARN", parsed.Verbosity, "Wrong value for Verbosity");
			Assert.IsNotNull(parsed.CopyWithArguments);
			Assert.AreEqual(@"'C:\Temp\Pluto'", parsed.CopyWithArguments.SrcFolder, "Wrong value for CopyWithArguments.SrcFolder");
		}

		#region Used Types

		public class TestArguments : CLIArguments
		{
			[LOVOptionDefinition("v", "verbosity", "Sets the verbosity of logging", true, new string[] { "DEBUG", "INFO", "WARN", "ERR"})]
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
	}
}
