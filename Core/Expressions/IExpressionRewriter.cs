// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  IExpressionRewriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    public interface IExpressionRewriter
    {
        Expression Rewrite(Expression expression);
        bool CanRewrite(Expression expression);
    }
}