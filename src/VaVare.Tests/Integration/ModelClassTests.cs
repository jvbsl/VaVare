﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VaVare.Statements;
using NUnit.Framework;
using VaVare.Builders;
using VaVare.Builders.BuildMembers;
using VaVare.Extensions;
using VaVare.Generators.Class;
using VaVare.Generators.Common;
using VaVare.Models;
using VaVare.Models.Properties;
using VaVare.Models.References;
using VaVare.Saver;

namespace VaVare.Tests.Integration
{
    [TestFixture]
    public class ModelClassTests
    {
        [Test]
        public void Test_CreateModelClass()
        {
            var classBuilder = new ClassBuilder("Cat", "Models");
            var @class = classBuilder
                .WithUsings("System")
                .WithConstructor(
                    ConstructorGenerator.Create(
                        "Cat",
                        BodyGenerator.Create(
                            Statement.Declaration.Assign("Name", ReferenceGenerator.Create(new VariableReference("name"))),
                            Statement.Declaration.Assign("Age", ReferenceGenerator.Create(new VariableReference("age")))),
                        new List<Parameter> { new Parameter("name", typeof(string)), new Parameter("age", typeof(int)) },
                        new List<Modifiers> { Modifiers.Public }))
                .WithProperties(
                    PropertyGenerator.Create(new AutoProperty("Name", typeof(string), PropertyTypes.GetAndSet, new List<Modifiers> { Modifiers.Public })),
                    PropertyGenerator.Create(new AutoProperty("Age", typeof(int), PropertyTypes.GetAndSet, new List<Modifiers> { Modifiers.Public })))
                       .Build();

            Assert.AreEqual(
                "usingSystem;namespaceModels{publicclassCat{publicCat(stringname,intage){Name=name;Age=age;}publicstringName{get;set;}publicintAge{get;set;}}}",
                @class.ToString());
        }

        [Test]
        public void Test_CreateClassWithEnum()
        {
            var classBuilder = new ClassBuilder("Cat", "Models");
            var @class = classBuilder
                .WithUsings("System")
                .With(new EnumBuildMember("MyEnum", new List<EnumMember> { new EnumMember("EnumValueOne", 2, new Attribute[] { new Attribute("MyAttribute"), }), new EnumMember("EnumValueTwo")}, new List<Modifiers> { Modifiers.Public }))
                .Build();

            Assert.AreEqual(
                "usingSystem;namespaceModels{publicclassCat{publicenumMyEnum{[MyAttribute]EnumValueOne=2,EnumValueTwo}}}",
                @class.ToString());
        }

        [Test]
        public void Test_CreateModelClassWithBodyProperties()
        {
            var classBuilder = new ClassBuilder("Cat", "Models");
            var @class = classBuilder
                .WithUsings("System")
                .WithFields(
                    new Field("_name", typeof(string), new List<Modifiers>() { Modifiers.Private}),
                    new Field("_age", typeof(int), new List<Modifiers>() { Modifiers.Private }))
                .WithConstructor(
                    ConstructorGenerator.Create(
                        "Cat",
                        BodyGenerator.Create(
                            Statement.Declaration.Assign("Name", ReferenceGenerator.Create(new VariableReference("name"))),
                            Statement.Declaration.Assign("Age", ReferenceGenerator.Create(new VariableReference("age")))),
                        new List<Parameter> { new Parameter("name", typeof(string)), new Parameter("age", typeof(int)) },
                        new List<Modifiers> { Modifiers.Public }))
                .WithProperties(
                    PropertyGenerator.Create(
                        new BodyProperty(
                            "Name",
                            typeof(string),
                            BodyGenerator.Create(Statement.Jump.Return(new VariableReference("_name"))), BodyGenerator.Create(Statement.Declaration.Assign("_name", new ValueKeywordReference())),
                            new List<Modifiers> { Modifiers.Public })),
                    PropertyGenerator.Create(
                        new BodyProperty(
                            "Age",
                            typeof(int),
                            BodyGenerator.Create(Statement.Jump.Return(new VariableReference("_age"))), BodyGenerator.Create(Statement.Declaration.Assign("_age", new ValueKeywordReference())),
                            new List<Modifiers> { Modifiers.Public })))
                       .Build();

            Assert.AreEqual(
                "usingSystem;namespaceModels{publicclassCat{privatestring_name;privateint_age;publicCat(stringname,intage){Name=name;Age=age;}publicstringName{get{return_name;}set{_name=value;}}publicintAge{get{return_age;}set{_age=value;}}}}",
                @class.ToString());
        }

        [Test]
        public void Test_CreateClassWithRegion()
        {
            var classBuilder = new ClassBuilder("Cat", "Models");
            var @class = classBuilder
                .WithUsings("System")
                .WithRegions(new RegionBuilder("MyRegion")
                    .WithProperties(PropertyGenerator.Create(new AutoProperty("Name", typeof(string), PropertyTypes.GetAndSet, new List<Modifiers> { Modifiers.Public })))
                    .Build())
                .Build();

            Assert.AreEqual(
                "usingSystem;namespaceModels{publicclassCat{#region MyRegion \npublicstringName{get;set;}#endregion}}",
                @class.ToString());
        }

        [Test]
        public void Test_CreateClassWithRegionWithMultipleMembers()
        {
            var classBuilder = new ClassBuilder("Cat", "Models");
            var @class = classBuilder
                .WithUsings("System")
                .WithRegions(new RegionBuilder("MyRegion")
                    .WithFields(
                        new Field("_name", typeof(string), new List<Modifiers>() { Modifiers.Private }),
                        new Field("_age", typeof(int), new List<Modifiers>() { Modifiers.Private }))
                    .WithProperties(PropertyGenerator.Create(new AutoProperty("Name", typeof(string), PropertyTypes.GetAndSet, new List<Modifiers> { Modifiers.Public })))
                    .WithConstructor(
                        ConstructorGenerator.Create(
                            "Cat",
                            BodyGenerator.Create(
                                Statement.Declaration.Assign("Name", ReferenceGenerator.Create(new VariableReference("name"))),
                                Statement.Declaration.Assign("Age", ReferenceGenerator.Create(new VariableReference("age")))),
                            new List<Parameter> { new Parameter("name", typeof(string)), new Parameter("age", typeof(int)) },
                            new List<Modifiers> { Modifiers.Public }))
                    .Build())
                .Build();

            Assert.AreEqual(
                "usingSystem;namespaceModels{publicclassCat{#region MyRegion \nprivatestring_name;privateint_age;publicstringName{get;set;}publicCat(stringname,intage){Name=name;Age=age;}#endregion}}",
                @class.ToString());
        }

        [Test]
        public void Test_CreateClassWithMethodThatHaveXmlDocumentation()
        {
            var classBuilder = new ClassBuilder("Cat", "Models");
            var @class = classBuilder
                .WithUsings("System")
                .WithMethods(new MethodBuilder("MyMethod")
                    .WithParameters(new Parameter("MyParameter", typeof(string), xmlDocumentation: "Some documentation"))
                    .WithSummary("Some summary")
                    .Build())
                .Build();

            Assert.AreEqual(
                "usingSystem;namespaceModels{publicclassCat{/// <summary>\n/// Some summary\n/// </summary>\n/// <param name=\"MyParameter\">Some documentation</param>\nvoidMyMethod(stringMyParameter){}}}",
                @class.ToString());
        }

        [Test]
        public void Test_CreateClassWithMethodThatHaveMultipleSUmmarysAndSingleLineComments()
        {
            var classBuilder = new ClassBuilder("Cat", "Models");
            var @class = classBuilder
                .WithUsings("System")
                .WithSummary("My class summary")
                .WithConstructor(ConstructorGenerator.Create(
                    "Cat",
                    BodyGenerator.Create(
                        Statement.Declaration.Assign("Name", ReferenceGenerator.Create(new VariableReference("name"))),
                        Statement.Declaration.Assign("Age", ReferenceGenerator.Create(new VariableReference("age")))),
                    new List<Parameter> { new Parameter("name", typeof(string)), new Parameter("age", typeof(int), xmlDocumentation: "My parameter") },
                    new List<Modifiers> { Modifiers.Public },
                    summary: "MyConstructor summary"))
                .WithProperties(new AutoProperty("MyProperty", typeof(int), PropertyTypes.GetAndSet, summary: "MyPropertySummary"))
                .WithFields(
                    new Field("_name", typeof(string), new List<Modifiers>() { Modifiers.Private }, summary: "My field summary")) 
                .WithMethods(new MethodBuilder("MyMethod")
                    .WithParameters(new Parameter("MyParameter", typeof(string)))
                    .WithBody(
                            BodyGenerator.Create(
                                Statement.Declaration.Declare("hello", typeof(int)).WithComment("My comment above").WithComment("hej"),
                                            Statement.Declaration.Declare("hello", typeof(int)).WithComment("My comment to the side", CommentPosition.Right)
                            ))
                    .Build())
                .Build();

            Assert.AreEqual(
                "usingSystem;namespaceModels{/// <summary>\n/// My class summary\n/// </summary>\npublicclassCat{/// <summary>\n/// MyConstructor summary\n/// </summary>\n/// <param name=\"age\">My parameter</param>\npublicCat(stringname,intage){Name=name;Age=age;}/// <summary>\n/// MyPropertySummary\n/// </summary>\nintMyProperty{get;set;}/// <summary>\n/// My field summary\n/// </summary>\nprivatestring_name;voidMyMethod(stringMyParameter){//hej\ninthello;inthello; //My comment to the side\n}}}",
                @class.ToString());
        }

        [Test]
        public void Test_CreateClassWithMethodThatHaveOverrideOperators()
        {
            var classBuilder = new ClassBuilder("Cat", "Models");
            var @class = classBuilder
                .WithUsings("System")
                .WithMethods(new MethodBuilder("MyMethod")
                    .WithModifiers(Modifiers.Public, Modifiers.Static)
                    .WithOperatorOverloading(Operators.Increment)
                    .WithParameters(new Parameter("MyParameter", typeof(string)))
                    .WithBody(
                            BodyGenerator.Create(
                                Statement.Declaration.Declare("hello", typeof(int)).WithComment("My comment above").WithComment("hej"),
                                            Statement.Declaration.Declare("hello", typeof(int)).WithComment("My comment to the side", CommentPosition.Right)
                            ))
                    .Build())
                .Build();

            Assert.AreEqual(
                "usingSystem;namespaceModels{publicclassCat{publicstaticMyMethodoperator++(stringMyParameter){//hej\ninthello;inthello; //My comment to the side\n}}}",
                @class.ToString());
        }
    }
}
