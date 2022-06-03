using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using DotLogix.Core.Expressions;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Expressions.Rewriters;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Expressions {
    public class NpgSqlSearchTermRewriter : SearchTermRewriter {
        protected override Expression GetLikeExpression(SearchTerm searchTerm, Lambda<string> value, ManyTerm<string> pattern) {
            var likeMethod = searchTerm.IgnoreCase ? (Func<string,string,bool>) EF.Functions.ILike : EF.Functions.Like;
            if(pattern.Count == 1) {
                var patternValue = pattern.Values[0];
                return EfFunctions.Call(likeMethod, value, patternValue);
            }

            var anyPatternValue = Expression.Parameter(typeof(string));
            var anyCallback = EfFunctions
                             .Call(likeMethod, value, anyPatternValue)
                             .ToLambda<string, bool>(anyPatternValue);
            
            return Lambdas.Constant(pattern.Values.AsEnumerable()).Any(anyCallback);
        }

        protected override Expression GetRegexExpression(SearchTerm searchTerm, Lambda<string> value, ManyTerm<string> pattern) {
            var options = searchTerm.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
            var regexPattern = pattern.Values.Count == 1
                                   ? pattern.Values[0]
                                   : $"(?:{string.Join(")|(?:", pattern)})";
            return value.MatchesRegex(regexPattern, options);
        }

        protected override Expression GetFuzzyExpression(SearchTerm searchTerm, Lambda<string> value, ManyTerm<string> pattern) {
            if(pattern.Values.Count == 1) {
                var similarity = EfFunctions.Call<string, string, double>(EF.Functions.TrigramsSimilarityDistance, value, pattern.Values[0]);
                return similarity.IsGreaterThanOrEqual(searchTerm.FuzzyThreshold);
            }

            var anyParameter = Expression.Parameter(typeof(string));
            var anyCallback = EfFunctions
                             .Call<string, string, double>(EF.Functions.TrigramsSimilarityDistance, value, anyParameter)
                             .IsGreaterThanOrEqual(searchTerm.FuzzyThreshold)
                             .ToLambda<string, bool>(anyParameter);

            return Lambdas.Constant(pattern.Values.AsEnumerable()).Any(anyCallback);
        }
    }
}