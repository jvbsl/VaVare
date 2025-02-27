﻿using NUnit.Framework;
using VaVare.Extensions.Naming;

namespace VaVare.Tests.Extensions.Naming
{
    [TestFixture]
    public class StringNamingExtensionsTests
    {
        [Test]
        public void FirstLetterToLowerCase_WhenHavingAString_ShouldSetLFirstLetterToLowerCase()
        {
            Assert.AreEqual("test", "Test".FirstLetterToLowerCase());
        }

        [Test]
        public void FirstLetterToLowerCase_WhenHavingAEmptyString_ShouldReturnSameString()
        {
            Assert.AreEqual(string.Empty, "".FirstLetterToLowerCase());
        }

        [Test]
        public void FirstLetterToUpperCase_WhenHavingAString_ShouldSetLFirstLetterToUpperCase()
        {
            Assert.AreEqual("Test", "test".FirstLetterToUpperCase());
        }

        [Test]
        public void FirstLetterToUpperCase_WhenHavingAEmptyString_ShouldReturnSameString()
        {
            Assert.AreEqual(string.Empty, "".FirstLetterToUpperCase());
        }
    }
}