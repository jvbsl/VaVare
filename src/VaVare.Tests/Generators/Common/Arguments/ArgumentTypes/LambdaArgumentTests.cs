﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using VaVare.Generators.Common;
using VaVare.Generators.Common.Arguments.ArgumentTypes;
using VaVare.Generators.Common.BinaryExpressions;
using VaVare.Models.References;
using VaVare.Statements;

namespace VaVare.Tests.Generators.Common.Arguments.ArgumentTypes
{
    [TestFixture]
    public class LambdaArgumentTests
    {
        [Test]
        public void GetArgumentSyntax_WhenCreatingEmpty_ShouldGetCorrectCode()
        {
            var argument = new LambdaArgument(Statement.Expression.Invoke("MyMethod").AsExpression(), "n");
            var syntax = argument.GetArgumentSyntax();

            Assert.IsInstanceOf<ArgumentSyntax>(syntax);
            Assert.AreEqual("n=>MyMethod()", syntax.ToString());
        }

        [Test]
        public void GetArgumentSyntax_WhenCreatingEmptyAsNamedArgument_ShouldGetCorrectCode()
        {
            var argument = new LambdaArgument(Statement.Expression.Invoke("MyMethod").AsExpression(), "n", namedArgument: "namedArgument");
            var syntax = argument.GetArgumentSyntax();

            Assert.IsInstanceOf<ArgumentSyntax>(syntax);
            Assert.AreEqual("namedArgument:n=>MyMethod()", syntax.ToString());
        }

        [Test]
        public void GetArgumentSyntax_WhenCreatingWithWithBlock_ShouldGetCorrectCode()
        {
            var block = BodyGenerator.Create(Statement.Expression.Invoke("MyMethod").AsStatement());

            var argument = new LambdaArgument(block, "n");
            var syntax = argument.GetArgumentSyntax();

            Assert.IsInstanceOf<ArgumentSyntax>(syntax);
            Assert.AreEqual("n=>{MyMethod();}", syntax.ToString());
        }

        [Test]
        public void GetArgumentSyntax_WhenCreatingWithBinaryExpression_ShouldGetCorrectCode()
        {
            var argument = new LambdaArgument(new ConditionalBinaryExpression(new MemberReference("n"), new MemberReference("true"), ConditionalStatements.Equal).GetBinaryExpression(), "n");
            var syntax = argument.GetArgumentSyntax();

            Assert.IsInstanceOf<ArgumentSyntax>(syntax);
            Assert.AreEqual("n=>n==true", syntax.ToString());
        }
    }
}
