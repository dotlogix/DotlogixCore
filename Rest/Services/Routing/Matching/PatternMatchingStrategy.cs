using DotLogix.Core.Rest.Http;
using DotLogix.Core.Utils.Patterns;

namespace DotLogix.Core.Rest.Services.Routing.Matching {
    public class PatternMatchingStrategy : RegexMatchingStrategy {
        private static readonly PatternParser PatternParser = PatternParser.Default.Clone();

        public PatternMatchingStrategy(string pattern, HttpMethods acceptedRequests, bool isRooted = false) : base(PatternParser.ToRegexPattern(pattern), acceptedRequests, isRooted) {
            Pattern = pattern;
        }
    }
}