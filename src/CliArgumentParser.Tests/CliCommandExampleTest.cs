using System;
using System.Collections.Generic;
using System.Text;
using CliArgumentParser;
using CliArgumentParser.Tests.TestCommands;
using CliArgumentParser.Validation;
using NUnit.Framework;

namespace CliArgumentParser.Tests
{
    public class CliCommandExampleTest : Test
    {

        [Test]
        public void Constructor_ThrowsEx_ifCommandIsNull()
        {
            //******* GIVEN
            ScanCommand cmd = null;
            string description = "my descr";

            //******* WHEN
            TryToRun(() => new CliCommandExample(description, cmd));


            //******* ASSERT
            AssertThatExceptionOfTypeOccurred<ArgumentNullException>();
            AssertExceptionHasStringPropertyEqualsTo<ArgumentNullException>(x => x.ParamName, "cmd");
            Assert.Pass();
        }

        [Test]
        public void Constructor_DoesNotThrowEx_ifCDescriptionIsNull()
        {
            //******* GIVEN
            ScanCommand cmd = new ScanCommand();
            string description = null;

            //******* WHEN
            var example = new CliCommandExample(description, cmd);

            //******* ASSERT
            Assert.That(example, Is.Not.Null);
            Assert.Pass();
        }
    }
}
