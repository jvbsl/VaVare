﻿using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VaVare.Builders.Base;

namespace VaVare.Builders
{
    public class NamespaceBuilder : BuilderBase<NamespaceBuilder>
    {
        public NamespaceBuilder(string @namespace)
          : base(@namespace)
        {
            if (string.IsNullOrEmpty(@namespace))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(@namespace));
            }
        }

        /// <summary>
        /// Build the namespace and return the generated code.
        /// </summary>
        /// <returns>The generated class.</returns>
        public CompilationUnitSyntax Build()
        {
            var @base = SyntaxFactory.CompilationUnit();
            @base = BuildUsings(@base);
            @base = BuildNamespace(@base);

            return @base;
        }
    }
}
