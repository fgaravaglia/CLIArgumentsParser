using System.Linq;
using CLIArgumentsParser.Core.Parsing;
using CLIArgumentsParser.Core.Verbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static CLIArgumentsParser.Tests.Parsing.ParserTestOnVerbs;

namespace CLIArgumentsParser.Tests.Verbs
{
	[TestClass]
	public class VerbParserTest : BaseUnitTest
	{
		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		[ExpectedException(typeof(InvalidCLIArgumentException))]
		public void ParsingDifferentVerb_ThrowsException()
		{
			//************* GIVEN
			var test = new VerbDefinitionAttribute("copy", "Copy all files from source to output fodler");
			var parser = new VerbParser(test, typeof(CopyArguments));

			//************* WHEN
			parser.Parse("del");

			//************* ASSERT
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingVerbStartingWithMinus_DoesNotThrowException()
		{
			//************* GIVEN
			var test = new VerbDefinitionAttribute("copy", "Copy all files from source to output fodler");
			var parser = new VerbParser(test, typeof(CopyArguments));

			//************* WHEN
			var value = parser.Parse("-copy");

			//************* ASSERT
			Assert.IsNotNull(value);
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingVerbWithMandatoryArgument_ReturnsThatFilledProperty()
		{
			//************* GIVEN
			VerbDefinitionAttribute test = typeof(CopyFilesWith1MandatoryOption).GetCustomAttributes(typeof(VerbDefinitionAttribute), true).First() as VerbDefinitionAttribute;
			var parser = new VerbParser(test, typeof(CopyFilesWith1MandatoryOption));

			//************* WHEN
			var value = parser.Parse(@"-copy2 --src='C:\Temp\My Folder\TTT'");

			//************* ASSERT
			Assert.IsNotNull(value);
			Assert.IsInstanceOfType(value, typeof(CopyFilesWith1MandatoryOption));
			CopyFilesWith1MandatoryOption verb = (CopyFilesWith1MandatoryOption)value;
			Assert.AreEqual(@"'C:\Temp\My Folder\TTT'", verb.SrcFolder);
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingVerbWithoutArguments_ReturnsThatClass()
		{
			//******** GIVEN
			var test = new VerbDefinitionAttribute("copy", "Copy all files from source to output fodler");
			var parser = new VerbParser(test, typeof(CopyArguments));

			//******** WHEN
			var parsed = parser.Parse("-copy");

			//******** ASSERT
			Assert.IsNotNull(parsed);
			Assert.IsInstanceOfType(parsed, typeof(CopyArguments));
		}
	}
}
