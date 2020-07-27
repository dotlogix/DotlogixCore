#region using
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
#endregion

namespace DotLogix.Core.Utils.Naming
{
    /// <summary>
    /// A base class to represent naming strategies
    /// </summary>
    public abstract class NamingStrategyBase : INamingStrategy
    {
        private static Regex WordPartRegex { get; } = new Regex("([A-Z]+(?=$|[^A-Za-z]|[A-Z][a-z])|[A-Z]?[a-z]+|[0-9]+)", RegexOptions.Compiled);
        private readonly object _lock = new object();
        private StringBuilder _stringBuilder;
        private readonly ConcurrentDictionary<string, string> _nameCache = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Rewrites the name according to the naming strategy
        /// The result will be cached and reused the next time the strategy is called with the same argument
        /// </summary>
        public string Rewrite(string value) {
            if (string.IsNullOrEmpty(value))
                return null;

            string ValueFactory(string v) {
                lock(_lock) {
                    _stringBuilder ??= new StringBuilder(50);
                    _stringBuilder.Clear();
                    _stringBuilder.EnsureCapacity(v.Length);
                    return RewriteValue(v, _stringBuilder);
                }
            }

            return _nameCache.GetOrAdd(value, ValueFactory);
        }

        /// <summary>
        /// Rewrites the name according to the naming strategy
        /// </summary>
        protected abstract string RewriteValue(string value, StringBuilder stringBuilder);


        /// <summary>
        /// Extracts word parts (A-Za-z0-9) out of a provided value
        /// </summary>
        protected static IEnumerable<StringSegment> ExtractWords(string value)
        {
            if (string.IsNullOrEmpty(value))
                yield break;
            
            foreach(Match match in WordPartRegex.Matches(value)) {
                if(match.Success && match.Length > 0)
                    yield return new StringSegment(value, match.Index, match.Length);
            }
        }
    }
}