﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Testura.Code.Statements;
using NUnit.Framework;
using Testura.Code.Builders;
using Testura.Code.Helpers.Class;
using Testura.Code.Helpers.Common;
using Testura.Code.Helpers.Common.References;
using Testura.Code.Models;

namespace Testura.Code.Tests.Integration
{
    [TestFixture]
    public class ModelClassTests
    {
        [Test]
        public void Test_Modela()
        {
            var classBuilder = new ClassBuilder("Cat", "Models");
            var @class = classBuilder
                .WithUsings("System")
                .WithProperties(Property.Create("Name", typeof(string), PropertyTypes.GetAndSet),
                    Property.Create("Age", typeof(int), PropertyTypes.GetAndSet))
                .WithConstructor(Class.Constructor("Cat",
                Body.Create(
                            Statement.Decleration.Assign("Name", Reference.Create(new VariableReference("name"))),
                            Statement.Decleration.Assign("Age", Reference.Create(new VariableReference("age")))
                            ),
                new List<Parameter> { new Parameter("name", typeof(string)), new Parameter("age", typeof(int)) }))
                       .Build(); 
            var m = @class.NormalizeWhitespace().ToString();
        }

        [Test]
        public void Test_Access_Model()
        {
            var classBuilder = new ClassBuilder("CatFarm", "Models");
            var @class = classBuilder
                .WithUsings("System")
                .WithMethods(
                    new MethodBuilder("Meow")
                        .WithParameters(new Parameter("cat", "Cat"))
                        .WithBody(Body.Create(
                                Statement.Decleration.DeclareAndAssign("meow", typeof(string), new VariableReference("cat", new MemberReference("MyProperty")))
                            ))
                        .Build()
                ).Build();
            var m = @class.NormalizeWhitespace().ToString();
        }
    }
}
