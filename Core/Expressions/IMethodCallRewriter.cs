// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  IMethodCallRewriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System.Linq.Expressions;
using System.Reflection;

namespace DotLogix.Core.Expressions {
    public interface IMethodCallRewriter : IExpressionRewriter
    {
        MethodInfo MatchesMethod { get; }
        Expression Rewrite(MethodCallExpression expression);
    }
}