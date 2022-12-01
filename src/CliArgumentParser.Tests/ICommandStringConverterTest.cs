using CliArgumentParser.Tests.TestCommands;
using NUnit.Framework;
using System;

namespace CliArgumentParser.Tests
{
    public class ICommandStringConverterTest : Test
    {
        ICommandStringConverter _Converter;

        #region Tests on DefinitionToString Method

        [Test]
        public void DefinitionToString_ReturnsTitleAndDescriptionAmdAvailableOptions()
        {
            //******* GIVEN
            var cmd = new ScanCommand();

            //******* WHEN
            this._Converter = new CommandStringConverter(cmd);
            var definition = Environment.NewLine.ToString() + this._Converter.DefinitionToString();

            //******* ASSERT
            Assert.That(definition, Is.Not.Null);
            Assert.That(definition.Length, Is.Not.EqualTo(0));
            var expectedString = @"
scan	Scan the target folder tree
Options:
-folder		Root folder to scan; 
-match-file		Expression to filter scan result; 
-match-exp		Expression to search into scanned files; 
-to		Persistence Driver for result; Valid Values: ['';'CSV']

";
            Assert.That(definition, Is.EqualTo(expectedString));
            Assert.Pass();
        }

        #endregion

        #region Tests on OptionsToString Method
        [Test]
        public void OptionsToString_ReturnsAvailableOptions()
        {
            //******* GIVEN
            var cmd = new ScanCommand();
            var expectedOptionsString = @"
Options:
-folder		Root folder to scan; 
-match-file		Expression to filter scan result; 
-match-exp		Expression to search into scanned files; 
-to		Persistence Driver for result; Valid Values: ['';'CSV']
";

            //******* WHEN
            this._Converter = new CommandStringConverter(cmd);
            var options = Environment.NewLine.ToString() + this._Converter.OptionsToString();

            //******* ASSERT
            Assert.That(options, Is.Not.Null);
            Assert.That(options.Length, Is.Not.EqualTo(0));
            Assert.That(options, Is.EqualTo(expectedOptionsString));
            Assert.Pass();
        }
        #endregion

        #region Tests on StatementToString Method
        [Test]
        public void StatementToString_ReturnsAvailableOptions()
        {
            //******* GIVEN
            var cmd = new ScanCommand();
            cmd.FileMatchExpression = ".csproj";
            cmd.ContentMatchExpression = "<latest>";
            cmd.Folder = @"c:\Temp\Test";
            cmd.PersistedTo = "CSV"; 
            var expectedString = @"scan -match-file=.csproj -match-exp=<latest> -folder=c:\Temp\Test -to=CSV ";

            //******* WHEN
            this._Converter = new CommandStringConverter(cmd);
            var convertedString = this._Converter.StatementToString();

            //******* ASSERT
            Assert.That(convertedString, Is.Not.Null);
            Assert.That(convertedString.Length, Is.Not.EqualTo(0));
            Assert.That(convertedString, Is.EqualTo(expectedString));
            Assert.Pass();
        }

        [Test]
        public void StatementToString_IgnoreNotMandatoryOptionsThatAreNotSet()
        {
            //******* GIVEN
            var cmd = new ListCommand();
            cmd.PackageNameFilter = "";
            cmd.Folder = @"c:\Temp\Test";
            var expectedString = @"list -folder=c:\Temp\Test ";

            //******* WHEN
            this._Converter = new CommandStringConverter(cmd);
            var convertedString = this._Converter.StatementToString();

            //******* ASSERT
            Assert.That(convertedString, Is.Not.Null);
            Assert.That(convertedString.Length, Is.Not.EqualTo(0));
            Assert.That(convertedString, Is.EqualTo(expectedString));
            Assert.Pass();
        }


        [Test]
        public void StatementToString_IgnoreNotMandatoryBooleanOptionsThatAreFalse()
        {
            //******* GIVEN
            var cmd = new ListCommand();
            cmd.PackageNameFilter = "";
            cmd.Folder = @"c:\Temp\Test";
            cmd.IsVerbose = false;
            var expectedString = @"list -folder=c:\Temp\Test ";
        
            //******* WHEN
            this._Converter = new CommandStringConverter(cmd);
            var convertedString = this._Converter.StatementToString();

            //******* ASSERT
            Assert.That(convertedString, Is.Not.Null);
            Assert.That(convertedString.Length, Is.Not.EqualTo(0));
            Assert.That(convertedString, Is.EqualTo(expectedString));
            Assert.Pass();
        }

        #endregion

        [Test]
        public void UsageToString_ReturnsTitleAndDescriptionAndExamples_IgnoringNotMandatoryOptions()
        {
            //******* GIVEN
            var cmd = new ListCommand();

            //******* WHEN
            this._Converter = new CommandStringConverter(cmd);
            var definition = Environment.NewLine.ToString() + this._Converter.UsageToString();

            //******* ASSERT
            Assert.That(definition, Is.Not.Null);
            Assert.That(definition.Length, Is.Not.EqualTo(0));
            var testString = definition.Split("Example:", 2)[1];
            var expectedString = @" List with no filter
list -verbose=False -folder=C:\Temp 
---------------------------------------------------------------
";
            Assert.That(testString, Is.EqualTo(expectedString));
            Assert.Pass();
        }
    }
}