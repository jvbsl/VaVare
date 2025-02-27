﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using VaVare.Models.References;

namespace VaVare.Tests.Models.References
{
    [TestFixture]
    public class ConstantReferenceTests
    {
        [Test]
        public void Constructor_WhenGivingANonNumericOrNonBoolean_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new ConstantReference(new List<string>()));
        }
    }
}
