// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  IParameterRewriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    public interface IParameterRewriter : IExpressionRewriter
    {
        ParameterExpression Parameter { get; }
        Expression Rewrite(ParameterExpression expression);
    }
}