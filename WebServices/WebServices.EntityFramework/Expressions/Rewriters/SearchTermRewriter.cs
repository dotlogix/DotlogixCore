#region
using System;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Expressions;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.WebServices.EntityFramework.Expressions.Rewriters {
    public class SearchTermRewriter : IMethodCallRewriter {
        protected static LambdaBuilder<DbFunctions> EfFunctions { get; } = LambdaBuilders.FromValue(EF.Functions);
        public MethodInfo MatchesMethod { get; } = TermExtensions.GetTermMethodInfo(typeof(SearchTerm));

        public Expression Rewrite(Expression expression) {
            return expression is MethodCallExpression callExpression
                       ? Rewrite(callExpression)
                       : expression;
        }

        public Expression Rewrite(MethodCallExpression expression) {
            var searchTerm = expression.Object.Evaluate<SearchTerm>();
            var value = LambdaBuilders.From<string>(expression.Arguments[0]);
            var pattern = searchTerm.Pattern;

            return searchTerm.Mode switch {
                SearchTermMode.Equals => GetEqualsExpression(searchTerm, value, pattern),
                SearchTermMode.StartsWith => GetStartsWithExpression(searchTerm, value, pattern),
                SearchTermMode.Contains => GetContainsExpression(searchTerm, value, pattern),
                SearchTermMode.EndsWith => GetEndsWithExpression(searchTerm, value, pattern),
                SearchTermMode.Like => GetLikeExpression(searchTerm, value, pattern),
                SearchTermMode.Wildcard => GetWildcardExpression(searchTerm, value, pattern),
                SearchTermMode.Regex => GetRegexExpression(searchTerm, value, pattern),
                SearchTermMode.Fuzzy => GetFuzzyExpression(searchTerm, value, pattern),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public virtual bool CanRewrite(Expression expression) {
            return expression is MethodCallExpression ex && (ex.Method == MatchesMethod);
        }


        protected virtual Expression GetEqualsExpression(SearchTerm searchTerm, LambdaBuilder<string> value, string pattern) {
            if(searchTerm.IgnoreCase) {
                value = value.ToLower();
                pattern = pattern.ToLower();
            }

            return value.Equal(pattern);
        }

        protected virtual Expression GetStartsWithExpression(SearchTerm searchTerm, LambdaBuilder<string> value, string pattern) {
            if(searchTerm.IgnoreCase) {
                value = value.ToLower();
                pattern = pattern.ToLower();
            }

            return value.StartsWith(pattern);
        }

        protected virtual Expression GetEndsWithExpression(SearchTerm searchTerm, LambdaBuilder<string> value, string pattern) {
            if(searchTerm.IgnoreCase) {
                value = value.ToLower();
                pattern = pattern.ToLower();
            }

            return value.EndsWith(pattern);
        }

        protected virtual Expression GetContainsExpression(SearchTerm searchTerm, LambdaBuilder<string> value, string pattern) {
            if(searchTerm.IgnoreCase) {
                value = value.ToLower();
                pattern = pattern.ToLower();
            }

            return value.Contains(pattern);
        }

        protected virtual Expression GetWildcardExpression(SearchTerm searchTerm, LambdaBuilder<string> value, string pattern) {
            pattern = pattern.Replace("*", "%");
            return GetLikeExpression(searchTerm, value, pattern);
        }

        protected virtual Expression GetLikeExpression(SearchTerm searchTerm, LambdaBuilder<string> value, string pattern) {
            if(searchTerm.IgnoreCase) {
                value = value.ToLower();
                pattern = pattern.ToLower();
            }

            return EfFunctions.Call<string, string, bool>(EF.Functions.Like, value, pattern);
        }

        protected virtual Expression GetRegexExpression(SearchTerm searchTerm, LambdaBuilder<string> value, string pattern) {
            throw new NotSupportedException("Regex operations are not supported");
        }

        protected virtual Expression GetFuzzyExpression(SearchTerm searchTerm, LambdaBuilder<string> value, string pattern) {
            throw new NotSupportedException("Fuzzy operations are not supported");
        }
    }
}
