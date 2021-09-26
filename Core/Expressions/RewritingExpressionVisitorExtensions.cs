// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  RewritingExpressionVisitorExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Expressions {
    public static class RewritingExpressionVisitorExtensions {
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, MethodInfo methodInfo, MethodInfo newMethod)
        {
            var rewriter = new MethodCallRewriter
            {
                MatchesMethod = methodInfo,
                Callback = expression =>
                           {
                               var instance = expression.Object;
                               var args = expression.Arguments.AsEnumerable();

                               if (methodInfo.IsStatic == newMethod.IsStatic)
                               {
                                   return Expression.Call(instance, newMethod, args);
                               }
                    
                               return newMethod.IsStatic
                                          ? Expression.Call(newMethod, instance.CreateEnumerable().Concat(args))
                                          : Expression.Call(expression.Arguments[0], newMethod, expression.Arguments.Skip(1));
                           }
            };
            visitor.Use(rewriter);
        }
        
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, Delegate source, Delegate target)
        {
            RewriteMethod(visitor, source.Method, target.Method);
        }
        
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, Type type, string method, string newMethod)
        {
            RewriteMethod(visitor, type.GetMethod(method), type.GetMethod(newMethod));
        }
        
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, Type type, string method, string newMethod, params Type[] parameterTypes)
        {
            RewriteMethod(visitor, type.GetMethod(method, parameterTypes), type.GetMethod(newMethod, parameterTypes));
        }
        
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, MethodInfo methodInfo, Func<MethodCallExpression, Expression> rewrite)
        {
            var rewriter = new MethodCallRewriter
            {
                MatchesMethod = methodInfo,
                Callback = rewrite
            };
            visitor.Use(rewriter);
        }

        
        public static void RewriteMember(this RewritingExpressionVisitor visitor, MemberInfo member, MemberInfo newMember)
        {
            var rewriter = new MemberRewriter
            {
                MemberInfo = member,
                Callback = p => Expression.MakeMemberAccess(p.Expression, newMember)
            };
            visitor.Use(rewriter);
        }
        
        public static void RewriteMember(this RewritingExpressionVisitor visitor, Type type, string member, string newMember)
        {
            RewriteMember(visitor, type.GetMember(member)[0], type.GetMember(newMember)[0]);
        }
        
        public static void RewriteMember(this RewritingExpressionVisitor visitor, MemberInfo memberInfo, Func<MemberExpression, Expression> rewrite)
        {
            var rewriter = new MemberRewriter
            {
                MemberInfo = memberInfo,
                Callback = rewrite
            };
            visitor.Use(rewriter);
        }
        
        public static void RewriteParameter(this RewritingExpressionVisitor visitor, ParameterExpression parameter, Expression newExpression)
        {
            var rewriter = new ParameterRewriter
            {
                Parameter = parameter,
                Callback = p => newExpression
            };
            visitor.Use(rewriter);
        }

        public static void RewriteParameter(this RewritingExpressionVisitor visitor, ParameterExpression parameter, Func<ParameterExpression, Expression> rewrite)
        {
            var rewriter = new ParameterRewriter
            {
                Parameter = parameter,
                Callback = rewrite
            };
            visitor.Use(rewriter);
        }
        
        public static void Rewrite(this RewritingExpressionVisitor visitor, Func<Expression, bool> condition, Func<Expression, Expression> rewrite)
        {
            var rewriter = new ExpressionRewriter
            {
                Condition = condition,
                Callback = rewrite
            };
            visitor.Use(rewriter);
        }
    }
}