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
			this._Parser = new OptionParser(Option.FromAttribute(test).OnTargetProperty(typeof(bool)));

			//************* WHEN
			this._Parser.Parse("--t");

			//************* ASSERT
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParseBoolOptionByCode_ReturnsTrue()
		{
			//************* GIVEN
			var test = new OptionDefinitionAttribute("s", "singleOutput", description: @"manage output as unique file instead to split it into several ones");
			this._Parser = new OptionParser(Option.FromAttribute(test).OnTargetProperty(typeof(bool)));

			//************* WHEN
			var returnedValue = this._Parser.Parse("--s");

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
			this._Parser = new OptionParser(Option.FromAttribute(test).OnTargetProperty(typeof(bool)));

			//************* WHEN
			var returnedValue = this._Parser.Parse("--singleOutput");

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
			this._Parser = new OptionParser(Option.FromAttribute(test).OnTargetProperty(typeof(string)));

			//************* WHEN
			var returnedValue = this._Parser.Parse("--v=MYVERBOSE");

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
			this._Parser = new OptionParser(Option.FromAttribute(test).OnTargetProperty(typeof(string)));

			//************* WHEN
			var returnedValue = this._Parser.Parse("--v");

			//************* ASSERT
			Assert.IsNotNull(returnedValue);
			Assert.IsInstanceOfType(returnedValue, typeof(string));
			Assert.AreEqual("DEFVAL", (string)returnedValue);
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParseOptionWithArgumentsAndLOV_ReturnsTheValueIfIsValid()
		{
			//************* GIVEN
			var test = new LOVOptionDefinitionAttribute("v", "verbosity", description: @"manage output as unique file instead to split it into several ones", mandatory: false, values: new string[] { "A", "B", "C" });
			this._Parser = new OptionParser(Option.FromAttribute(test).OnTargetProperty(typeof(string)));

			//************* WHEN
			var returnedValue = this._Parser.Parse("--v=A");

			//************* ASSERT
			Assert.IsNotNull(returnedValue);
			Assert.IsInstanceOfType(returnedValue, typeof(string));
			Assert.AreEqual("A", (string)returnedValue);
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		[ExpectedException(typeof(InvalidCLIArgumentException))]
		public void ParseOptionWithArgumentsAndLOV_ThrowsExpcetionIfValueIsNotValid()
		{
			//************* GIVEN
			var test = new LOVOptionDefinitionAttribute("v", "verbosity", description: @"manage output as unique file instead to split it into several ones", mandatory: false, values: new string[] { "A", "B", "C" });
			this._Parser = new OptionParser(Option.FromAttribute(test).OnTargetProperty(typeof(string)));

			//************* WHEN
			var returnedValue = this._Parser.Parse("--v=DDDDDD");
		}
	}
}
