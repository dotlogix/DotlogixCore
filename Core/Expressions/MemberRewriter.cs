// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  MemberRewriter.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DotLogix.Core.Expressions {
    public class MemberRewriter : IMemberRewriter
    {
        public Func<MemberExpression, Expression> Callback { get; set; }
        public MemberInfo MemberInfo { get; set; }

        public Expression Rewrite(Expression expression) => Rewrite((MemberExpression) expression);

        public virtual Expression Rewrite(MemberExpression expression)
        {
            return Callback.Invoke(expression);
        }

        public virtual bool CanRewrite(Expression expression)
        {
            return expression is MemberExpression ex
                && ex.Member == MemberInfo;
        }
    }
}