using NUnit.Framework;
using System;

namespace Nx.Core.UnitTests.Conditions
{
    [TestFixture]
    public class WhenUsingConditions
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RequireShouldThrowWhenPredicateIsNegative()
        {
            Condition.Require<InvalidOperationException>(false);
        }

        [Test]
        public void RequireShouldNotThrowWhenPredicateIsPositive()
        {
            Condition.Require<InvalidOperationException>(true);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotNullShouldThrowWhenArgumentIsNull()
        {
            Condition.ArgumentNotNull<object>(null, "argument");
        }

        [Test]
        public void NotNullShouldNotThrowWhenArguementIsNotNull()
        {
            Condition.ArgumentNotNull<object>(new object(), "argument");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotNullOrEmptyShouldThrowWhenArgumentIsNull()
        {
            Condition.ArgumentNotNullOrEmpty(null, "argument");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotNullOrEmptyShouldThrowWhenArgumentIsEmpty()
        {
            Condition.ArgumentNotNullOrEmpty(string.Empty, "argument");
        }

        [Test]
        public void NotNullOrEmptyShouldNotThrowWhenArgumentIsNotNullOrEmpty()
        {
            Condition.ArgumentNotNullOrEmpty("abc", "argument");
        }
    }
}