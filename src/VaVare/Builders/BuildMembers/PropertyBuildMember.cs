﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VaVare.Builders.BuildMembers
{
    public class PropertyBuildMember : IBuildMember
    {
        private readonly IEnumerable<PropertyDeclarationSyntax> _propertyDeclarationSyntaxs;

        public PropertyBuildMember(IEnumerable<PropertyDeclarationSyntax> propertyDeclarationSyntaxs)
        {
            _propertyDeclarationSyntaxs = propertyDeclarationSyntaxs;
        }

        public SyntaxList<MemberDeclarationSyntax> AddMember(SyntaxList<MemberDeclarationSyntax> members)
        {
            foreach (var propertyDeclarationSyntax in _propertyDeclarationSyntaxs)
            {
                members = members.Add(propertyDeclarationSyntax);
            }

            return members;
        }
    }
}
