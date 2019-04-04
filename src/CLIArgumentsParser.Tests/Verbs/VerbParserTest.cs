using CLIArgumentsParser.Core.Options;
using CLIArgumentsParser.Core.Parsing;
using CLIArgumentsParser.Core.Verbs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
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
            var parser = new VerbParser(Verb.FromAttribute(test).OnTargetProperty(typeof(CopyArguments)));

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
            var parser = new VerbParser(Verb.FromAttribute(test).OnTargetProperty(typeof(CopyArguments)));

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
            var verbModel = Verb.FromAttribute(test).OnTargetProperty(typeof(CopyFilesWith1MandatoryOption));
            verbModel.AddOptionFromAttribute(new OptionDefinitionAttribute("src", "source", "source folder to use to copy files", mandatory: true), typeof(string));
            var parser = new VerbParser(verbModel);

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
            var parser = new VerbParser(Verb.FromAttribute(test).OnTargetProperty(typeof(CopyArguments)));

            //******** WHEN
            var parsed = parser.Parse("-copy");

            //******** ASSERT
            Assert.IsNotNull(parsed);
            Assert.IsInstanceOfType(parsed, typeof(CopyArguments));
        }

        [TestMethod]
        [TestCategory(BaseUnitTest.UNIT)]
        public void ParsingVerbStartingWithDoubleMinus_ReturnsKeyIsValid()
        {
            //******** GIVEN
            var test = new VerbDefinitionAttribute("copy", "Copy all files from source to output fodler");
            var parser = new VerbParser(Verb.FromAttribute(test).OnTargetProperty(typeof(CopyArguments)));

            //******** WHEN
            string errorMessage;
            var parsed = parser.IsKeyValid("--copy", out errorMessage);

            //******** ASSERT
            Assert.IsTrue(parsed);
        }

        [TestMethod]
        [TestCategory(BaseUnitTest.UNIT)]
        public void ParsingVerbStartingWithMinus_ReturnsKeyIsValid()
        {
            //******** GIVEN
            var test = new VerbDefinitionAttribute("copy", "Copy all files from source to output fodler");
            var parser = new VerbParser(Verb.FromAttribute(test).OnTargetProperty(typeof(CopyArguments)));

            //******** WHEN
            string errorMessage;
            var parsed = parser.IsKeyValid("-copy", out errorMessage);

            //******** ASSERT
            Assert.IsTrue(parsed);
        }

        [TestMethod]
        [TestCategory(BaseUnitTest.UNIT)]
        [ExpectedException(typeof(ArgumentException))]
        public void ParsingNullString_ThrowsError()
        {
            //******** GIVEN
            var test = new VerbDefinitionAttribute("copy", "Copy all files from source to output fodler");
            var parser = new VerbParser(Verb.FromAttribute(test).OnTargetProperty(typeof(CopyArguments)));

            //******** WHEN
            var parsed = parser.Parse("");
        }
    }
}
