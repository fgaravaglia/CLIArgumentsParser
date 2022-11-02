using System;
using System.Collections.Generic;
using System.Text;
using CliArgumentParser;
using CliArgumentParser.Tests.TestCommands;
using NUnit.Framework;

namespace CliArgumentParser.Tests
{
    public class CliArgumentParserTest : Test
    {
        CliArgumentParser _Parser;

        public override void OnTestSetup()
        {
            base.OnTestSetup();

            var factory = new CommandFactory();
            factory.RegisterCommand<ScanCommand>("scan");
            factory.RegisterCommand<PrintCommand>("print");
            this._Parser = factory.InstanceFromFactory().UsingDefaultErrorManagement();
        }

        [Test]
        public void ParseArguments_WithNoArgs_HasErrorsButNoExceptionIsthrown()
        {
            //******* GIVEN
            var myargs = Array.Empty<string>();

            //******* WHEN
            TryToRun(() => this._Parser.ParseArguments(myargs));
            

            //******* ASSERT
            AssertThatNoExceptionOccurred();
            Assert.That(this._Parser.HasError, Is.EqualTo(true));
            Assert.Pass();
        }

        [Test]
        public void ParseArguments_WithFirstArgStarintWithIndicator_HasErrorsButNoExceptionIsthrown()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "-scan",
                @"-folder=c:\temp\test\uc.q8f.ua.blotter", 
                "-match-file=.csproj",
               @"-match-exp=\nugetpackages\",
               "-to=csv"
            };

            //******* WHEN
            TryToRun(() => this._Parser.ParseArguments(myargs));


            //******* ASSERT
            AssertThatNoExceptionOccurred();
            Assert.That(this._Parser.HasError, Is.EqualTo(true));
            Assert.Pass();
        }

        [Test]
        public void ParseArguments_WithOptionNotStarintWithIndicator_HasErrorsButNoExceptionIsthrown()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "scan",
                @"folder=c:\temp\test\uc.q8f.ua.blotter",
                "-match-file=.csproj",
               @"-match-exp=\nugetpackages\",
               "-to=csv"
            };

            //******* WHEN
            TryToRun(() => this._Parser.ParseArguments(myargs));


            //******* ASSERT
            AssertThatNoExceptionOccurred();
            Assert.That(this._Parser.HasError, Is.EqualTo(true));
            Assert.Pass();
        }


        [Test]
        public void ParseArguments_WithValidArgs_HasNoErrors()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "scan",
                @"-folder=c:\temp\test\uc.q8f.ua.blotter", 
                "-match-file=.csproj",
               @"-match-exp=\nugetpackages\",
               "-to=csv"
            };

            //******* WHEN
            this._Parser.ParseArguments(myargs);


            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(false));
            Assert.Pass();
        }

        [Test]
        public void RunCallback_ExecutesTheActionOnlyIfTypeIsMatching()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "scan",
                @"-folder=c:\temp\test\uc.q8f.ua.blotter",
                "-match-file=.csproj",
               @"-match-exp=\nugetpackages\",
               "-to=csv"
            };
            this._Parser.ParseArguments(myargs);
            bool isExecuted = false;

            //******* WHEN
            this._Parser.RunCallbackFor<ScanCommand>(x => { isExecuted = true; });

            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(false));
            Assert.That(isExecuted, Is.EqualTo(true));
            Assert.Pass();
        }

        [Test]
        public void RunCallback_DoesNotExecuteTheActionIfTypeIsNotMatching()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "print",
                @"-verbosity=DEBUG"
            };
            this._Parser.ParseArguments(myargs);
            bool isExecuted = false;

            //******* WHEN
            this._Parser.RunCallbackFor<ScanCommand>(x => { isExecuted = true; });

            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(false));
            Assert.That(isExecuted, Is.EqualTo(false));
            Assert.Pass();
        }
    }
}
