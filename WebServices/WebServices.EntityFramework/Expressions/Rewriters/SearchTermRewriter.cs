#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DotLogix.Core.Expressions;
using DotLogix.Core.Expressions.Rewriters;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DotLogix.WebServices.EntityFramework.Expressions.Rewriters {
    public class SearchTermRewriter : IMethodCallRewriter {
        private static readonly MethodInfo TargetMethodInfo = TermExtensions.GetTermMethodInfo(typeof(RangeTerm<>));
        protected static Lambda<DbFunctions> EfFunctions { get; } = Lambdas.Constant(EF.Functions);

        public Expression Rewrite(Expression _, MethodInfo method, IReadOnlyList<Expression> arguments) {
            if(method != TargetMethodInfo)
                return default;

            var (termExpression, valueExpression) = arguments;
            var searchTerm = termExpression.Evaluate<SearchTerm>();
            var value = Lambdas.From<string>(valueExpression);
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
        protected virtual Expression GetEqualsExpression(SearchTerm searchTerm, Lambda<string> value, ManyTerm<string> pattern) {
            if(pattern.Count == 1) {
                var patternValue = pattern.Values[0];
                if(searchTerm.IgnoreCase) {
                    value = value.ToLower();
                    patternValue = patternValue.ToLower();
                }
                return value.IsEqualTo(patternValue);
            }

            IEnumerable<string> patternValues = pattern.Values;
            if(searchTerm.IgnoreCase) {
                value = value.ToLower();
                patternValues = patternValues.Select(p => p.ToLower()).ToArray();
            }
            return Lambdas.Constant(patternValues).Contains(value);
        }
        protected virtual Expression GetStartsWithExpression(SearchTerm searchTerm, Lambda<string> value, ManyTerm<string> pattern) {
            if(pattern.Count == 1) {
                var patternValue = pattern.Values[0];
                if(searchTerm.IgnoreCase) {
                    value = value.ToLower();
                    patternValue = patternValue.ToLower();
                }
                return value.StartsWith(patternValue);
            }

            IEnumerable<string> patternValues = pattern.Values;
            if(searchTerm.IgnoreCase) {
                value = value.ToLower();
                patternValues = patternValues.Select(p => p.ToLower()).ToArray();
            }

            var anyPatternValue = Expression.Parameter(typeof(string));
            var anyCallback = value.StartsWith(anyPatternValue).ToLambda<string, bool>(anyPatternValue);
            return Lambdas.Constant(patternValues).Any(anyCallback);
        }
        protected virtual Expression GetEndsWithExpression(SearchTerm searchTerm, Lambda<string> value, ManyTerm<string> pattern) {
            if(pattern.Count == 1) {
                var patternValue = pattern.Values[0];
                if(searchTerm.IgnoreCase) {
                    value = value.ToLower();
                    patternValue = patternValue.ToLower();
                }
                return value.EndsWith(patternValue);
            }

            IEnumerable<string> patternValues = pattern.Values;
            if(searchTerm.IgnoreCase) {
                value = value.ToLower();
                patternValues = patternValues.Select(p => p.ToLower()).ToArray();
            }

            var anyPatternValue = Expression.Parameter(typeof(string));
            var anyCallback = value.EndsWith(anyPatternValue).ToLambda<string, bool>(anyPatternValue);
            return Lambdas.Constant(patternValues).Any(anyCallback);
        }
        protected virtual Expression GetContainsExpression(SearchTerm searchTerm, Lambda<string> value, ManyTerm<string> pattern) {
            if(pattern.Count == 1) {
                var patternValue = pattern.Values[0];
                if(searchTerm.IgnoreCase) {
                    value = value.ToLower();
                    patternValue = patternValue.ToLower();
                }
                return value.Contains(patternValue);
            }

            IEnumerable<string> patternValues = pattern.Values;
            if(searchTerm.IgnoreCase) {
                value = value.ToLower();
                patternValues = patternValues.Select(p => p.ToLower()).ToArray();
            }

            var anyPatternValue = Expression.Parameter(typeof(string));
            var anyCallback = value.Contains(anyPatternValue).ToLambda<string, bool>(anyPatternValue);
            return Lambdas.Constant(patternValues).Any(anyCallback);
        }
        protected virtual Expression GetWildcardExpression(SearchTerm searchTerm, Lambda<string> value, ManyTerm<string> pattern) {
            pattern = new ManyTerm<string>(pattern.Select(p => p.Replace("*", "%")));
            return GetLikeExpression(searchTerm, value, pattern);
        }
        protected virtual Expression GetLikeExpression(SearchTerm searchTerm, Lambda<string> value, ManyTerm<string> pattern) {
            if(pattern.Count == 1) {
                var patternValue = pattern.Values[0];
                if(searchTerm.IgnoreCase) {
                    value = value.ToLower();
                    patternValue = patternValue.ToLower();
                }
                return EfFunctions.Call<string, string, bool>(EF.Functions.Like, value, patternValue);
            }

            IEnumerable<string> patternValues = pattern.Values;
            if(searchTerm.IgnoreCase) {
                value = value.ToLower();
                patternValues = patternValues.Select(p => p.ToLower()).ToArray();
            }

            var anyPatternValue = Expression.Parameter(typeof(string));

            var anyCallback = EfFunctions
                             .Call<string, string, bool>(EF.Functions.Like, value, anyPatternValue)
                             .ToLambda<string, bool>(anyPatternValue);
            
            return Lambdas.Constant(patternValues).Any(anyCallback);
        }
        protected virtual Expression GetRegexExpression(SearchTerm searchTerm, Lambda<string> value, ManyTerm<string> pattern) {
            throw new NotSupportedException("Regex operations are not supported");
        }
        protected virtual Expression GetFuzzyExpression(SearchTerm searchTerm, Lambda<string> value, ManyTerm<string> pattern) {
            throw new NotSupportedException("Fuzzy operations are not supported");
        }
    }
}
