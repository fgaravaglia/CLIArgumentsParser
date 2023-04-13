using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CliArgumentParser;
using CliArgumentParser.ErrorManagement;
using CliArgumentParser.Tests.TestCommands;
using CliArgumentParser.Validation;
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
            factory.RegisterCommand<TestWithFlagCommand>("flag");
            factory.RegisterCommand<ListCommand>("list");
            factory.RegisterCommand<TestWithListOfValues>("test");
            factory.RegisterCommand<TestWithLookupValue>("testWithDomainValue");
            this._Parser = factory.InstanceFromFactory().UsingDefaultErrorManagement();
        }

        [Test]
        public void Constructor_ThrowsEx_ifFactoryIsNull()
        {
            //******* GIVEN
            var myargs = Array.Empty<string>();

            //******* WHEN
            TryToRun(() => new CliArgumentParser(null, null));


            //******* ASSERT
            AssertThatExceptionOfTypeOccurred<ArgumentNullException>();
            AssertExceptionHasStringPropertyEqualsTo<ArgumentNullException>(x => x.ParamName, "factory");
            Assert.Pass();
        }

        [Test]
        public void Constructor_ThrowsEx_ifValidatorIsNull()
        {
            //******* GIVEN
            ICliCommandValidator validator = null;
            var factory = new CommandFactory();
            factory.RegisterCommand<ScanCommand>("scan");
            factory.RegisterCommand<PrintCommand>("print");

            //******* WHEN
            TryToRun(() => new CliArgumentParser(factory, validator));


            //******* ASSERT
            AssertThatExceptionOfTypeOccurred<ArgumentNullException>();
            AssertExceptionHasStringPropertyEqualsTo<ArgumentNullException>(x => x.ParamName, "validator");
            Assert.Pass();
        }

        #region Test on SetErrorCallback Method

        [Test]
        public void SetErrorCallback_ThrowException_IfCallbackIsNull()
        {
            //******* GIVEN
            Func<Exception, int> callback = null;

            //******* WHEN
            TryToRun(() => this._Parser.SetErrorCallback(callback));


            //******* ASSERT
            AssertThatExceptionOfTypeOccurred<ArgumentNullException>();
            AssertExceptionHasStringPropertyEqualsTo<ArgumentNullException>(x => x.ParamName, "callback");
            Assert.Pass();
        }

        [Test]
        public void SetErrorCallback_ThrowException_IfCallbackFOrExceptionTypeExists()
        {
            //******* GIVEN
            Func<Exception, int> callback = x => 1;

            //******* WHEN
            TryToRun(() => this._Parser.SetErrorCallback(callback));

            //******* ASSERT
            AssertThatExceptionOfTypeOccurred<ArgumentException>();
            AssertExceptionHasStringPropertyEqualsTo<ArgumentException>(x => x.ParamName, "callback");
            Assert.Pass();
        }

        #endregion

        #region Test on ParseArguments Method

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
        public void ParseArguments_WithFlag_HasNoErrors()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "flag",
                @"-verbose"
            };

            //******* WHEN
            this._Parser.ParseArguments(myargs);


            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(false));
            Assert.Pass();
        }

        [Test]
        public void ParseArguments_WithFlagNotMandatory_HasNoErrors()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "list",
                @"-folder=C:\temp",
                @"-verbose"
            };

            //******* WHEN
            this._Parser.ParseArguments(myargs);


            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(false));
            Assert.Pass();
        }

        [Test]
        public void ParseArguments_WithoutFlagNotMandatory_HasNoErrors()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "list",
                @"-folder=C:\temp",
            };

            //******* WHEN
            this._Parser.ParseArguments(myargs);


            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(false));
            Assert.Pass();
        }

        // [Test]
        // public void ParseArguments_WithLIstOfValues_HasNoErrors()
        // {
        //     //******* GIVEN
        //     var myargs = new string[]
        //     {
        //         "test",
        //         @"-values=1;2;3"
        //     };

        //     //******* WHEN
        //     this._Parser.ParseArguments(myargs);

        //     //******* ASSERT
        //     Assert.That(this._Parser.HasError, Is.EqualTo(false));
        //     Assert.Pass();
        // }

        [Test]
        public void ParseArguments_WithLIstOfValues_WithNoSeprator_HasErrors()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "test",
                @"-values=1"
            };

            //******* WHEN
            this._Parser.ParseArguments(myargs);

            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(true));
            Assert.IsInstanceOf<WrongOptionUsageException>(this._Parser.OccurredError);
            Assert.True(this._Parser.OccurredError.Message.StartsWith("Wrong Usage: invalid option -values for test", StringComparison.InvariantCultureIgnoreCase), 
                        $"wrong Exception Message. Found <{this._Parser.OccurredError.Message}>");
            Assert.Pass();
        }


        [Test]
        public void ParseArguments_WithDomainValue_WithNotValidValue_HasErrors()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "testWithDomainValue",
                @"-domain=pippo"
            };

            //******* WHEN
            this._Parser.ParseArguments(myargs);

            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(true));
            Assert.IsInstanceOf<WrongOptionUsageException>(this._Parser.OccurredError);
            Assert.True(this._Parser.OccurredError.Message.StartsWith("Wrong Usage: invalid option -domain for testWithDomainValue - Value PIPPO is not valid for List of values", StringComparison.InvariantCultureIgnoreCase),
                        $"wrong Exception Message. Found <{this._Parser.OccurredError.Message}>");
            Assert.Pass();
        }

        [Test]
        public void ParseArguments_WithDomainValue_WithValidValue_HasNoErrors()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "testWithDomainValue",
                @"-domain=VAL1"
            };

            //******* WHEN
            this._Parser.ParseArguments(myargs);

            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(false));
            Assert.Pass();
        }
        #endregion

        #region Test on RunCallback Method
        [Test]
        public void RunCallback_ThrowsException_IfCallbackIsNull()
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
            Action<ScanCommand> callback = null;

            //******* WHEN
            TryToRun(() => this._Parser.RunCallbackFor<ScanCommand>(callback));

            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(true));
            Assert.That(this._Parser.OccurredError.GetType(), Is.EqualTo(typeof(ArgumentNullException)));
            Assert.That(((ArgumentNullException)this._Parser.OccurredError).ParamName, Is.EqualTo("callback"));
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

        [Test]
        public void RunCallback_ExecutesTheAction_UsingCommandWithFlag()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "flag",
                @"-verbose"
            };
            this._Parser.ParseArguments(myargs);
            bool isExecuted = false;
            TestWithFlagCommand parsedCmd = null;

            //******* WHEN
            this._Parser.RunCallbackFor<TestWithFlagCommand>(x => { isExecuted = true; parsedCmd = x; });

            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(false));
            Assert.That(isExecuted, Is.EqualTo(true));
            Assert.False(parsedCmd == null);
            Assert.True(parsedCmd.IsVerbose);
            Assert.Pass();
        }

        [Test]
        public void RunCallback_ExecutesTheAction_UsingCommandWithoutNotMandatoryOptions()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "list",
                @"-folder=c:\temp"
            };
            this._Parser.ParseArguments(myargs);
            bool isExecuted = false;
            ListCommand parsedCmd = null;

            //******* WHEN
            this._Parser.RunCallbackFor<ListCommand>(x => { isExecuted = true; parsedCmd = x; });

            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(false));
            Assert.That(isExecuted, Is.EqualTo(true));
            Assert.False(parsedCmd == null);
            Assert.False(parsedCmd.IsVerbose);
            Assert.That(parsedCmd.Folder, Is.EqualTo(@"c:\temp"));
            Assert.True(string.IsNullOrEmpty(parsedCmd.PackageNameFilter));
            Assert.Pass();
        }

        #endregion

        #region Test on CaseWhen Method
        [Test]
        public void CaseWhen_UsingCommandWithoutNotMandatoryOptions()
        {
            //******* GIVEN
            var myargs = new string[]
            {
                "list",
                @"-folder=c:\temp"
            };
            this._Parser.ParseArguments(myargs);
            bool isExecuted = false;
            ListCommand parsedCmd = null;

            //******* WHEN
            var exitCode = this._Parser.CaseWhen<ListCommand>(x => { isExecuted = true; parsedCmd = x; }).Return();

            //******* ASSERT
            Assert.That(this._Parser.HasError, Is.EqualTo(false));
            Assert.That(exitCode, Is.EqualTo(0));
            Assert.That(isExecuted, Is.EqualTo(true));
            Assert.False(parsedCmd == null);
            Assert.False(parsedCmd.IsVerbose);
            Assert.That(parsedCmd.Folder, Is.EqualTo(@"c:\temp"));
            Assert.True(string.IsNullOrEmpty(parsedCmd.PackageNameFilter));
            Assert.Pass();
        }

        // [Test]
        // public void CaseWhen_UsingCommandWithListOption()
        // {
        //     //******* GIVEN
        //     var myargs = new string[]
        //     {
        //         "test",
        //         @"-values=1;2;3"
        //     };
        //     this._Parser.ParseArguments(myargs);
        //     bool isExecuted = false;
        //     TestWithListOfValues parsedCmd = null;

        //     //******* WHEN
        //     var exitCode = this._Parser.CaseWhen<TestWithListOfValues>(x => { isExecuted = true; parsedCmd = x; }).Return();

        //     //******* ASSERT
        //     Assert.That(this._Parser.HasError, Is.EqualTo(false), "Parser has failed!");
        //     Assert.That(exitCode, Is.EqualTo(0), "Wrong exit Code");
        //     Assert.That(isExecuted, Is.EqualTo(true), "Wrong Behaviour: lambda has not been executed");
        //     Assert.IsNotNull(parsedCmd);
        //     Assert.That(parsedCmd.Values.Count(), Is.EqualTo(3), "List has been parsed with errors");
        //     Assert.True(parsedCmd.Values.ToList().Contains("1"), "List has been parsed with errors: missing element 1");
        //     Assert.True(parsedCmd.Values.ToList().Contains("2"), "List has been parsed with errors: missing element 2");
        //     Assert.True(parsedCmd.Values.ToList().Contains("3"), "List has been parsed with errors: missing element 3");
        //     Assert.Pass();
        // }

        #endregion
    }
}
