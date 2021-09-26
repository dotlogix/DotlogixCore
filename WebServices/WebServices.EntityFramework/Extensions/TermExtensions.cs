using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Terms;

namespace DotLogix.WebServices.EntityFramework.Extensions {
    public static class TermExtensions {
        private static readonly IReadOnlyCollection<MethodInfo> Methods = typeof(TermExtensions).GetRuntimeMethods().AsReadOnlyCollection();
    
        public static bool Matches(this SearchTerm term, string value) {
            var pattern = term.Pattern;
            var comparison = term.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            switch(term.Mode) {
                case SearchTermMode.Equals: {
                    return string.Equals(value, pattern, comparison);
                }
                case SearchTermMode.StartsWith: {
                    return value.StartsWith(pattern, comparison);
                }
                case SearchTermMode.Contains: {
                    return value.IndexOf(pattern, comparison) >= 0;
                }
                case SearchTermMode.EndsWith: {
                    return value.EndsWith(pattern, comparison);
                }
                case SearchTermMode.Fuzzy: {
                    var distance = value.GetLevenshteinDistance(value);
                    return distance < (1 - term.FuzzyThreshold);
                }
                case SearchTermMode.Like: {
                    pattern = "^" + pattern.Replace("%", ".*").Replace("_", ".") + "$";
                    goto case SearchTermMode.Regex;
                }
                case SearchTermMode.Wildcard: {
                    pattern = "^" + pattern.Replace("*", ".*").Replace("_", ".") + "$";
                    goto case SearchTermMode.Regex;
                }
                case SearchTermMode.Regex: {
                    var regex = new Regex(pattern, term.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                    return regex.IsMatch(value);
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(term.Mode), "The provided filter type is not supported");
            }
        }
        
        public static bool Matches<T>(this RangeTerm<T> term, T value) {
            var comparer = Comparer<T>.Default;
            return (term.Min.IsUndefined || comparer.Compare(value, term.Min.Value) >= 0)
                && (term.Max.IsUndefined || comparer.Compare(value, term.Max.Value) <= 0);
        }
        
        public static bool Matches<T>(this ManyTerm<T> term, T value) {
            switch(term.Count) {
                case 0:
                    return true;
                case 1:
                    return Equals(term.Values[0], value);
                default:
                    return term.Values.Contains(value);
            }
        }

        public static MethodInfo GetTermMethodInfo(Type expectedTermType) {
            return Methods
               .FirstOrDefault(m => {
                                   var termType = m.GetParameters().FirstOrDefault()?.ParameterType;
                                   return expectedTermType.IsGenericTypeDefinition
                                              ? termType.IsAssignableToGeneric(expectedTermType)
                                              : termType.IsAssignableTo(expectedTermType);
                               }
                              );
        }
    }
}