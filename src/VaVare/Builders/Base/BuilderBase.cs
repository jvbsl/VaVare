﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VaVare.Builders.BuilderHelpers;
using VaVare.Builders.BuildMembers;

namespace VaVare.Builders.Base
{
    public class BuilderBase<TBuilder>
        where TBuilder : BuilderBase<TBuilder>
    {
        private readonly NamespaceHelper _namespaceHelper;
        private readonly UsingHelper _usingHelper;
        private readonly MemberHelper _memberHelper;

        public BuilderBase(string @namespace)
        {
            _memberHelper = new MemberHelper();
            _usingHelper = new UsingHelper();
            _namespaceHelper = new NamespaceHelper(@namespace);
        }

        /// <summary>
        /// Set the using directives.
        /// </summary>
        /// <param name="usings">A set of wanted using directive names.</param>
        /// <returns>The current builder</returns>
        public TBuilder WithUsings(params string[] usings)
        {
            _usingHelper.AddUsings(usings);
            return (TBuilder)this;
        }

        /// <summary>
        /// Add build members that will be generated.
        /// </summary>
        /// <param name="buidMembers">Build members to add</param>
        /// <returns>The current builder</returns>
        public TBuilder With(params IBuildMember[] buildMembers)
        {
            _memberHelper.AddMembers(buildMembers);
            return (TBuilder)this;
        }

        protected CompilationUnitSyntax BuildUsings(CompilationUnitSyntax @base)
        {
            return _usingHelper.BuildUsings(@base);
        }

        protected CompilationUnitSyntax BuildNamespace(CompilationUnitSyntax @base, params MemberDeclarationSyntax[] members)
        {
            return _namespaceHelper.BuildNamespace(@base, members);
        }

        protected CompilationUnitSyntax BuildNamespace(CompilationUnitSyntax @base, SyntaxList<MemberDeclarationSyntax> members)
        {
            return _namespaceHelper.BuildNamespace(@base, members);
        }

        protected CompilationUnitSyntax BuildNamespace(CompilationUnitSyntax @base)
        {
            return _namespaceHelper.BuildNamespace(@base, _memberHelper.BuildSyntaxList());
        }

        protected TypeDeclarationSyntax BuildMembers(TypeDeclarationSyntax type)
        {
            return _memberHelper.BuildMembers(type);
        }

        protected CompilationUnitSyntax BuildMembers(CompilationUnitSyntax compilationUnitSyntax)
        {
            return _memberHelper.BuildMembers(compilationUnitSyntax);
        }
    }
}
