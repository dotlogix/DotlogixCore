// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  ParameterReplacementVisitor.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System.Linq.Expressions;

namespace DotLogix.Core.Expressions {
    public class ParameterReplacementVisitor : ExpressionVisitor
    {
        public ParameterExpression SourceExpression { get; set; }
        public ParameterExpression TargetExpression { get; set; }

        public ParameterReplacementVisitor(ParameterExpression sourceExpression, ParameterExpression targetExpression)
        {
            SourceExpression = sourceExpression;
            TargetExpression = targetExpression;
        }

        public override Expression Visit(Expression node)
        {
            if (node is ParameterExpression p && p == SourceExpression)
            {
                return TargetExpression;
            }
            return base.Visit(node);
        }
    }
}