using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using DotLogix.Core.Expressions;
using DotLogix.WebServices.Core.Terms;
using DotLogix.WebServices.EntityFramework.Expressions.Rewriters;
using Microsoft.EntityFrameworkCore;

namespace DotLogix.WebServices.EntityFramework.Expressions {
    public class NpgSqlSearchTermRewriter : SearchTermRewriter {
        protected override Expression GetLikeExpression(SearchTerm searchTerm, LambdaBuilder<string> value, string pattern) {
            var likeFunc = searchTerm.IgnoreCase ? (Func<string, string, bool>)EF.Functions.ILike : EF.Functions.Like;
            return EfFunctions.Call<string, string, bool>(likeFunc, value, pattern);
        }

        protected override Expression GetRegexExpression(SearchTerm searchTerm, LambdaBuilder<string> value, string pattern) {
            var options = searchTerm.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
            return value.MatchesRegex(pattern, options);
        }

        protected override Expression GetFuzzyExpression(SearchTerm searchTerm, LambdaBuilder<string> value, string pattern) {
            var similarity = EfFunctions.Call<string, string, double>(EF.Functions.TrigramsSimilarityDistance, value, pattern);
            return similarity.IsGreaterThanOrEqual(searchTerm.FuzzyThreshold);
        }
    }
}