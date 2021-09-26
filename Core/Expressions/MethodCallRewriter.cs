// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  MethodCallRewriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DotLogix.Core.Expressions {
    public class MethodCallRewriter : IMethodCallRewriter
    {
        public Func<MethodCallExpression, Expression> Callback { get; set; }
        public MethodInfo MatchesMethod { get; set; }

        public Expression Rewrite(Expression expression) {
            return expression is MethodCallExpression mce ? Rewrite(mce) : null;
        }

        public virtual Expression Rewrite(MethodCallExpression expression)
        {
            return Callback.Invoke(expression);
        }
        public virtual bool CanRewrite(Expression expression)
        {
            if (!(expression is MethodCallExpression ex))
                return false;

            if (MatchesMethod.IsGenericMethodDefinition == false)
                return ex.Method == MatchesMethod;
            
            return ex.Method.IsGenericMethod
                && ex.Method.GetGenericMethodDefinition() == MatchesMethod;

        }
    }
}