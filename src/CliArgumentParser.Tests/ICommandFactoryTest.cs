using CliArgumentParser.ErrorManagement;
using CliArgumentParser.Tests.TestCommands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliArgumentParser.Tests
{
    public class ICommandFactoryTest : Test
    {
        ICommandFactory _Factory;

        public override void OnTestSetup()
        {
            base.OnTestSetup();

            this._Factory = new CommandFactory();
        }

        [Test]
        public void RegisterCommand_ThrowsArgumentNullExceptionIfVerbIsNull()
        {
            //******* GIVEN
            string verb = null;

            //******* WHEN
            TryToRun(() => this._Factory.RegisterCommand<ScanCommand>(verb));

            //******* ASSERT
            AssertThatExceptionOfTypeOccurred<ArgumentNullException>();
            Assert.Pass();
        }

        [Test]
        public void RegisterCommand_ThrowsArgumentNullExceptionIfVerbIsAlreadyRegistered()
        {
            //******* GIVEN
            string verb = "scan";
            this._Factory.RegisterCommand<ScanCommand>(verb);

            //******* WHEN
            TryToRun(() => this._Factory.RegisterCommand<ScanCommand>(verb));

            //******* ASSERT
            AssertThatExceptionOfTypeOccurred<ArgumentException>();
            Assert.Pass();
        }

        [Test]
        public void GetCommand_ThrowsUnknownVerbExceptionIfVerbIsNotRegistered()
        {
            //******* GIVEN
            string verb = "not-registered-verb";

            //******* WHEN
            TryToRun(() => this._Factory.GetCommand(verb));

            //******* ASSERT
            AssertThatExceptionOfTypeOccurred<UnknownVerbException>();
            Assert.Pass();
        }

        
    }
}
