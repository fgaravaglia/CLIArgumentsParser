using System;
using CLIArgumentsParser.Core;
using CLIArgumentsParser.Core.Verbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLIArgumentsParser.Tests.Parsing
{
	[TestClass]
	public class ParserTestOnVerbs : BaseUnitTest
	{
		Parser _Parser;

		protected override void InitializeMyTestData()
		{
			base.InitializeMyTestData();

			this._Parser = new Parser();
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingVerbWithoutArguments_ReturnsThatClass()
		{
			//******** GIVEN
			string[] arguments = new string[] { "-copy" };
			Exception thrownEx = null;

			//******** WHEN
			TestArguments parsed = this._Parser.Parse<TestArguments>(arguments);

			//******** ASSERT
			thrownEx = this._Parser.GetLastError();
			string message = "Expected no exception at this stage";
			if (thrownEx != null)
				message += (": " + thrownEx.Message);
			Assert.IsNull(thrownEx, message);
			Assert.IsNotNull(parsed);
		}

		#region Used Types

		public class TestArguments : CLIArguments
		{
			public CopyArguments Copy { get; set; }
		}

		[VerbDefinition("copy", "copy files from SRC to OUTPUT")]
		public class CopyArguments : CLIArguments
		{
		}

		#endregion
	}
}
