using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CliArgumentParser.Tests
{
    public abstract class Test
    {
        protected Exception _OccurredEx;

        [SetUp]
        public virtual void OnTestSetup()
        {
            this._OccurredEx = null;
        }

        protected void TryToRun(Action act)
        {
            try
            {
                act.Invoke();
            }
            catch (Exception ex)
            {
                this._OccurredEx = ex;
            }
        }

        protected void AssertThatExceptionOfTypeOccurred<T>() where T : Exception
        {
            Assert.That(this._OccurredEx, Is.Not.Null);
            Assert.That(this._OccurredEx.GetType(), Is.EqualTo(typeof(T)));
        }

        protected void AssertThatNoExceptionOccurred() 
        {
            Assert.That(this._OccurredEx, Is.Null);
        }

        protected void AssertExceptionHasStringPropertyEqualsTo<T>(Func<T, string> propertySelector, string expectedValue) where T : Exception
        {
            T casted = (T)this._OccurredEx;
            Assert.That(propertySelector.Invoke(casted), Is.EqualTo(expectedValue));
        }

        protected void AssertExceptionStartsMessageWith<T>(string expectedValue) where T : Exception
        {
            T casted = (T)this._OccurredEx;
            Assert.That(casted.Message.StartsWith(expectedValue), Is.EqualTo(true));
        }

    }
}
