using System.Collections.Generic;
using System.Linq;
using CLIArgumentsParser.Core.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLIArgumentsParser.Tests.Parsing
{
	[TestClass]
	public class TokenGeneratorTest : BaseUnitTest
	{
		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void EmptyString_ReturnsEmptyList()
		{
			//************* GIVEN
			var generator = TokenGenerator.Default();
			string textString = "";
			List<Token> expectedTokens = new List<Token>();

			//************* WHEN
			var tokens = generator.TokenizeThisString(textString);

			//************* GIVEN
			Assert.IsNotNull(tokens);
			Assert.AreEqual(expectedTokens.Count, tokens.Count());
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void StringStartingWithOptionIdentifier_Returns1TokenWithoutIdentifier()
		{
			//************* GIVEN
			var generator = TokenGenerator.Default();
			string textString = @"--source='C:\Temp\My Test Folder\TTTTT'";
			List<Token> expectedTokens = new List<Token>()
			{
				Token.New("source", @"'C:\Temp\My Test Folder\TTTTT'")
			};

			//************* WHEN
			var tokens = generator.TokenizeThisString(textString).ToList();

			//************* GIVEN
			Assert.IsNotNull(tokens);
			Assert.AreEqual(expectedTokens.Count, tokens.Count);
			AssertTokensAreEqual(expectedTokens[0], tokens[0]);
		}

		[TestMethod]
		[TestCategory(BaseUnitTest.UNIT)]
		public void StringStartingWithOptionIdentifier_Returns1TokenWithIdentifierIfEnabled()
		{
			//************* GIVEN
			var generator = TokenGenerator.Default().AddingIdentifierToTokenNames();
			string textString = @"--source='C:\Temp\My Test Folder\TTTTT'";
			List<Token> expectedTokens = new List<Token>()
			{
				Token.New("--source", @"'C:\Temp\My Test Folder\TTTTT'")
			};

			//************* WHEN
			var tokens = generator.TokenizeThisString(textString).ToList();

			//************* GIVEN
			Assert.IsNotNull(tokens);
			Assert.AreEqual(expectedTokens.Count, tokens.Count);
			AssertTokensAreEqual(expectedTokens[0], tokens[0]);
		}

		private void AssertTokensAreEqual(Token expected, Token parsed)
		{
			Assert.AreEqual(expected.Name, parsed.Name, "Wrong token name!!");
			Assert.AreEqual(expected.Value, parsed.Value, "Wrong token Value!!");
		}
	}
}
