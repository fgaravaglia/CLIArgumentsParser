using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLIArgumentsParser.Tests.Options
{
	[TestClass]
	public class OptionParserTest : BaseUnitTest
	{
		OptionParser _Parser;

		protected override void InitializeMyTestData()
		{
			base.InitializeMyTestData();
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		[ExpectedException(typeof(InvalidCLIArgumentException))]
		public void ParseDifferentOption_ThrowsException()
		{
			//************* GIVEN
			var test = new OptionDefinitionAttribute("s", "singleOutput", description: @"manage output as unique file instead to split it into several ones");
			this._Parser = new OptionParser(test);

			//************* WHEN
			this._Parser.Parse("-t", typeof(bool));

			//************* ASSERT
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParseBoolOptionByCode_ReturnsTrue()
		{
			//************* GIVEN
			var test = new OptionDefinitionAttribute("s", "singleOutput", description: @"manage output as unique file instead to split it into several ones");
			this._Parser = new OptionParser(test);

			//************* WHEN
			var returnedValue = this._Parser.Parse("-s", typeof(bool));

			//************* ASSERT
			Assert.IsNotNull(returnedValue);
			Assert.IsInstanceOfType(returnedValue, typeof(bool));
			Assert.IsTrue((bool)returnedValue);
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParseBoolOptionByLongCode_ReturnsTrue()
		{
			//************* GIVEN
			var test = new OptionDefinitionAttribute("s", "singleOutput", description: @"manage output as unique file instead to split it into several ones");
			this._Parser = new OptionParser(test);

			//************* WHEN
			var returnedValue = this._Parser.Parse("-singleOutput", typeof(bool));

			//************* ASSERT
			Assert.IsNotNull(returnedValue);
			Assert.IsInstanceOfType(returnedValue, typeof(bool));
			Assert.IsTrue((bool)returnedValue);
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParseOptionWithArguments_ReturnsTheValue()
		{
			//************* GIVEN
			var test = new OptionDefinitionAttribute("v", "verbosity", description: @"manage output as unique file instead to split it into several ones", mandatory: false, defaultValue: "DEFVAL");
			this._Parser = new OptionParser(test);

			//************* WHEN
			var returnedValue = this._Parser.Parse("-v MYVERBOSE", typeof(string));

			//************* ASSERT
			Assert.IsNotNull(returnedValue);
			Assert.IsInstanceOfType(returnedValue, typeof(string));
			Assert.AreEqual("MYVERBOSE", (string)returnedValue);
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParseOptionWithArgumentsAndDefault_ReturnsTheDefaultIfThereIsNoValue()
		{
			//************* GIVEN
			var test = new OptionDefinitionAttribute("v", "verbosity", description: @"manage output as unique file instead to split it into several ones", mandatory: false, defaultValue: "DEFVAL");
			this._Parser = new OptionParser(test);

			//************* WHEN
			var returnedValue = this._Parser.Parse("-v", typeof(string));

			//************* ASSERT
			Assert.IsNotNull(returnedValue);
			Assert.IsInstanceOfType(returnedValue, typeof(string));
			Assert.AreEqual("DEFVAL", (string)returnedValue);
		}
	}
}
