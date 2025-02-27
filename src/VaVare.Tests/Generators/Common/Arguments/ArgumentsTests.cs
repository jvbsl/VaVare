﻿using NUnit.Framework;
using VaVare.Generators.Common;
using VaVare.Generators.Common.Arguments.ArgumentTypes;
using Assert = NUnit.Framework.Assert;

namespace VaVare.Tests.Generators.Common.Arguments
{
    [TestFixture]
    public class ArgumentsTests
    {
        [Test]
        public void Create_WhenNotProvidingAnyArguments_ShouldGetEmptyString()
        {
            Assert.AreEqual("()", ArgumentGenerator.Create().ToString());
        }

        [Test]
        public void Create_WhenNotProvidingSingleArgument_ShouldContainArgument()
        {
            Assert.AreEqual("(1)", ArgumentGenerator.Create(new ValueArgument(1)).ToString());
        }

        [Test]
        public void Create_WhenNotProvidingMultipleArgument_ShouldContainArguments()
        {
            Assert.AreEqual("(1,2)", ArgumentGenerator.Create(new ValueArgument(1), new ValueArgument(2)).ToString());
        }
    }
}