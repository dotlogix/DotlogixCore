// ==================================================
// Copyright 2014-2021(C), DotLogix
// File:  RewritingExpressionVisitorExtensions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 08.05.2021 23:35
// LastEdited:  26.09.2021 22:15
// ==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Extensions;

namespace DotLogix.Core.Expressions.Rewriters {
    /// <summary>
    /// </summary>
    public static class RewritingExpressionVisitorExtensions {
        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IMethodCallRewriter)" />
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, MethodInfo methodInfo, Func<Expression, IReadOnlyList<Expression>, Expression> rewrite) {
            Expression RewriteMethodInternal(Expression instance, MethodInfo method, IReadOnlyList<Expression> arguments) {
                if(method != methodInfo) {
                    return default;
                }
                return rewrite(instance, arguments);
            }
            visitor.Add(new MethodCallRewriter(RewriteMethodInternal));
        }

        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IMethodCallRewriter)" />
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, MethodInfo methodInfo, MethodInfo newMethod) {
            Expression RewriteMethodInternal(Expression instance, MethodInfo method, IReadOnlyList<Expression> arguments) {
                if(method != methodInfo)
                    return default;
                
                if(methodInfo.IsStatic == newMethod.IsStatic) {
                    return Expression.Call(instance, newMethod, arguments);
                }

                return newMethod.IsStatic
                    ? Expression.Call(newMethod, arguments.Prepend(instance))
                    : Expression.Call(arguments[0], newMethod, arguments.Skip(1));
            }
            visitor.Add(new MethodCallRewriter(RewriteMethodInternal));
        }
        
        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IMethodCallRewriter)" />
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, MethodInfo methodInfo, LambdaExpression lambdaExpression) {
            Expression RewriteMethodInternal(Expression instance, MethodInfo method, IReadOnlyList<Expression> arguments) {
                if(method != methodInfo) {
                    return default;
                }

                if(arguments.Count == lambdaExpression.Parameters.Count) {
                    return Expression.Invoke(lambdaExpression, arguments);
                }
                
                if(method.IsStatic == false && arguments.Count + 1 == lambdaExpression.Parameters.Count) {
                    return Expression.Invoke(lambdaExpression, arguments.Prepend(instance));
                }
                
                return default;
            }
            visitor.Add(new MethodCallRewriter(RewriteMethodInternal));
        }

        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IMethodCallRewriter)" />
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, Type type, string method, LambdaExpression lambdaExpression) {
            var parameterTypes = lambdaExpression.Parameters.Select(p => p.Type).ToArray();
            RewriteMethod(visitor, type.GetMethod(method, parameterTypes), lambdaExpression);
        }

        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IMethodCallRewriter)" />
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, Delegate source, Delegate target) {
            RewriteMethod(visitor, source.Method, target.Method);
        }

        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IMethodCallRewriter)" />
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, Type type, string method, string newMethod) {
            RewriteMethod(visitor, type.GetMethod(method), type.GetMethod(newMethod));
        }

        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IMethodCallRewriter)" />
        public static void RewriteMethod(this RewritingExpressionVisitor visitor, Type type, string method, string newMethod, params Type[] parameterTypes) {
            RewriteMethod(visitor, type.GetMethod(method, parameterTypes), type.GetMethod(newMethod, parameterTypes));
        }

        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IMemberRewriter)" />
        public static void RewriteMember(this RewritingExpressionVisitor visitor, MemberInfo memberInfo, Func<Expression, Expression> rewrite) {
            Expression RewriteMemberInternal(Expression instance, MemberInfo member) {
                return member == memberInfo ? rewrite(instance) : default;
            }
            visitor.Add(new MemberRewriter(RewriteMemberInternal));
        }

        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IMemberRewriter)" />
        public static void RewriteMember(this RewritingExpressionVisitor visitor, MemberInfo memberInfo, MemberInfo newMemberInfo) {
            Expression RewriteMemberInternal(Expression instance, MemberInfo member) {
                return member == memberInfo ? Expression.MakeMemberAccess(instance, newMemberInfo) : default(Expression);
            }
            visitor.Add(new MemberRewriter(RewriteMemberInternal));
        }

        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IMemberRewriter)" />
        public static void RewriteMember(this RewritingExpressionVisitor visitor, Type type, string member, string newMember) {
            RewriteMember(visitor, type.GetMember(member)[0], type.GetMember(newMember)[0]);
        }


        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IParameterRewriter)" />
        public static void RewriteParameter(this RewritingExpressionVisitor visitor, ParameterExpression parameter, Expression newExpression) {
            visitor.Add(new ParameterReplacingRewriter(parameter, newExpression));
        }

        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IParameterRewriter)" />
        public static void RewriteParameter(this RewritingExpressionVisitor visitor, IReadOnlyCollection<ParameterExpression> parameters, IReadOnlyCollection<Expression> newExpressions) {
            visitor.Add(new ParameterReplacingRewriter(parameters.AsArray(), newExpressions.AsArray()));
        }

        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IParameterRewriter)" />
        public static void RewriteParameter(this RewritingExpressionVisitor visitor, ParameterExpression parameter, Func<ParameterExpression, Expression> rewrite) {
            Expression RewriteParameterInternal(ParameterExpression expression) {
                if(expression != parameter) {
                    return default;
                }
                return rewrite(expression);
            }
            visitor.Add(new ParameterRewriter(RewriteParameterInternal));
        }


        /// <inheritdoc cref="RewritingExpressionVisitor.Add(DotLogix.Core.Expressions.Rewriters.IExpressionRewriter)" />
        public static void Rewrite(this RewritingExpressionVisitor visitor, Func<Expression, Expression> rewrite, Func<Expression, bool> condition) {
            var rewriter = new ExpressionRewriter(e => condition.Invoke(e) ? rewrite.Invoke(e) : default);
            visitor.Add(rewriter);
        }
    }
}