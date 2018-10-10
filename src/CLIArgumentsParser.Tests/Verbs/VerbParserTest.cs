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
			var parser = new VerbParser(test);

			//************* WHEN
			parser.Parse("del", typeof(string));

			//************* ASSERT
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingVerbStartingWithMinus_DoesNotThrowException()
		{
			//************* GIVEN
			var test = new VerbDefinitionAttribute("copy", "Copy all files from source to output fodler");
			var parser = new VerbParser(test);

			//************* WHEN
			var value = parser.Parse("-copy", typeof(object));

			//************* ASSERT

		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void ParsingVerbWithoutArguments_ReturnsThatClass()
		{
			//******** GIVEN
			var test = new VerbDefinitionAttribute("copy", "Copy all files from source to output fodler");
			var parser = new VerbParser(test);

			//******** WHEN
			var parsed = parser.Parse("-copy", typeof(CopyArguments));

			//******** ASSERT
			Assert.IsNotNull(parsed);
			Assert.IsInstanceOfType(parsed, typeof(CopyArguments));
		}
	}
}
