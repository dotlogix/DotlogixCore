#region using
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DotLogix.Core.Extensions;
using DotLogix.Core.Utils.Pooling;
#endregion

namespace DotLogix.Core.Utils.Naming {
    /// <summary>
    ///     A base class to represent naming strategies
    /// </summary>
    public abstract class NamingStrategyBase : INamingStrategy {
        private static readonly ObjectPool<StringBuilder> StringBuilderPool = new(100, () => new StringBuilder(256), (sb) => sb.Clear());

        /// <summary>
        ///     Rewrites the name according to the naming strategy
        /// </summary>
        public string Rewrite(string value) {
            if(string.IsNullOrEmpty(value))
                return null;

            var sb = StringBuilderPool.RentOrCreate();
            var rewritten = RewriteValue(value, sb);
            StringBuilderPool.Return(sb);
            return rewritten;
        }

        /// <summary>
        ///     Rewrites the name according to the naming strategy
        /// </summary>
        protected abstract string RewriteValue(string value, StringBuilder stringBuilder);


        /// <summary>
        ///     Extracts word parts (A-Za-z0-9) out of a provided value
        /// </summary>
        protected static IEnumerable<ArraySegment<char>> ExtractWords(string value) {
            if(string.IsNullOrEmpty(value))
                yield break;

            //([A-Z]+(?=$|[^A-Za-z]|[A-Z][a-z])|[A-Z]?[a-z]+|[0-9]+)
            var chrArray = value.ToCharArray();
            var startIdx = -1;
            UnicodeCategory? previousCategory = null;
            for(var i = 0; i < chrArray.Length; i++) {
                var chr = chrArray[i];

                var isWordChar = chr.LaysBetween('0', '9') || chr.LaysBetween('a', 'z') || chr.LaysBetween('A', 'Z');
                if(isWordChar == false) {
                    if((startIdx >= 0) && ((i - startIdx) > 0)) {
                        yield return new ArraySegment<char>(chrArray, startIdx, i - startIdx);
                        previousCategory = null;
                        startIdx = -1;
                    }

                    continue;
                }

                var category = char.GetUnicodeCategory(chr);

                if(startIdx < 0) {
                    startIdx = i;
                    previousCategory = category;
                    continue;
                }

                if(previousCategory == category) continue;

                switch(previousCategory) {
                    case UnicodeCategory.DecimalDigitNumber:
                        switch(category) {
                            case UnicodeCategory.LowercaseLetter:
                            case UnicodeCategory.UppercaseLetter:
                                yield return new ArraySegment<char>(chrArray, startIdx, i - startIdx);
                                startIdx = i;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case UnicodeCategory.LowercaseLetter:
                        switch(category) {
                            case UnicodeCategory.DecimalDigitNumber:
                                yield return new ArraySegment<char>(chrArray, startIdx, i - startIdx);
                                startIdx = i;
                                break;
                            case UnicodeCategory.UppercaseLetter:
                                yield return new ArraySegment<char>(chrArray, startIdx, i - startIdx);
                                startIdx = i;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    case UnicodeCategory.UppercaseLetter:
                        switch(category) {
                            case UnicodeCategory.DecimalDigitNumber:
                                yield return new ArraySegment<char>(chrArray, startIdx, i - startIdx);
                                startIdx = i;
                                break;
                            case UnicodeCategory.LowercaseLetter:
                                if((i - startIdx) > 1) {
                                    yield return new ArraySegment<char>(chrArray, startIdx, i - startIdx - 1);
                                    startIdx = i - 1;
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                }

                previousCategory = category;
            }

            if((startIdx >= 0) && (startIdx < chrArray.Length)) yield return new ArraySegment<char>(chrArray, startIdx, chrArray.Length - startIdx);


            //foreach(Match match in WordPartRegex.Matches(value)) {
            //    if(match.Success && match.Length > 0)
            //        yield return new StringSegment(value, match.Index, match.Length);
            //}
        }

        /// <inheritdoc cref="Equals(object)"/>
        protected bool Equals(NamingStrategyBase other) {
            return other.GetType() != GetType();
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj)) {
                return false;
            }

            if(ReferenceEquals(this, obj)) {
                return true;
            }

            if(obj.GetType() != GetType()) {
                return false;
            }

            return Equals((NamingStrategyBase)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return GetType().GetHashCode();
        }
    }
}
