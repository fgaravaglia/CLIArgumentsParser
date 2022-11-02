using CliArgumentParser.ErrorManagement;
using CliArgumentParser.Tests.TestCommands;
using CliArgumentParser.Validation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliArgumentParser.Tests.Validation
{
    public class CLICommandValidatorTest : Test
    {
        CliCommandValidator _Validator;

        public override void OnTestSetup()
        {
            base.OnTestSetup();

            _Validator = new CliCommandValidator();
        }

        [Test]
        public void Validator_ThrowsEx_IfCommandHasNotAMandatoryOption()
        {
            //******* GIVEN
            ScanCommand cmd = GetValidCommand();
            cmd.Folder = null;
            string option = "-folder";

            //******* WHEN
            TryToRun(() => this._Validator.AssertIsValid(cmd));

            //******* ASSERT
            AssertThatExceptionOfTypeOccurred<WrongOptionUsageException>();
            AssertExceptionHasStringPropertyEqualsTo<WrongOptionUsageException>(x => x.Option, option);
            AssertExceptionStartsMessageWith<WrongOptionUsageException>($"Wrong Usage: invalid option { option} for scan");
            Assert.Pass();
        }

        [Test]
        public void Validator_ThrowsEx_IfCommandHasAnOptionWithNotValidValue()
        {
            //******* GIVEN
            ScanCommand cmd = GetValidCommand();
            cmd.PersistedTo = "NOTVALID";
            string option = "-to";

            //******* WHEN
            TryToRun(() => this._Validator.AssertIsValid(cmd));

            //******* ASSERT
            AssertThatExceptionOfTypeOccurred<WrongOptionUsageException>();
            AssertExceptionHasStringPropertyEqualsTo<WrongOptionUsageException>(x => x.Option, option);
            AssertExceptionStartsMessageWith<WrongOptionUsageException>($"Wrong Usage: invalid option { option} for scan");
            Assert.Pass();
        }

        static ScanCommand GetValidCommand()
        {
            ScanCommand cmd = new ScanCommand();
            cmd.Folder = @"c:\temp\test\uc.q8f.ua.blotter";
            cmd.FileMatchExpression = @".csproj";
            cmd.ContentMatchExpression = @"\nugetpackages\";
            cmd.PersistedTo = @"CSV";
            return cmd;
        }
    }
}
