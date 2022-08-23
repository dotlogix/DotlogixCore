using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DotLogix.Core.Extensions;
using DotLogix.WebServices.Core.Terms;

namespace DotLogix.WebServices.EntityFramework.Extensions; 

public static class TermExtensions {
    private static readonly IReadOnlyCollection<MethodInfo> Methods = typeof(TermExtensions).GetRuntimeMethods().AsReadOnlyCollection();

    public static ManyTerm<T> ToManyTerm<T>(this IEnumerable<T> values) => new(values);

    public static bool Matches(this SearchTerm term, string value) {
        IEnumerable<string> patterns = term.Pattern;
        var comparison = term.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        switch(term.Mode) {
            case SearchTermMode.Equals: {
                return patterns.Any(pattern => string.Equals(value, pattern, comparison));
            }
            case SearchTermMode.StartsWith: {
                return patterns.Any(pattern => value.StartsWith(pattern, comparison));
            }
            case SearchTermMode.Contains: {
                return patterns.Any(pattern => value.IndexOf(pattern, comparison) >= 0);
            }
            case SearchTermMode.EndsWith: {
                return patterns.Any(pattern => value.EndsWith(pattern, comparison));
            }
            case SearchTermMode.Fuzzy: {
                var distance = patterns.Min(value.GetLevenshteinDistance);
                return distance < (1 - term.FuzzyThreshold);
            }
            case SearchTermMode.Like: {
                patterns = patterns.Select(pattern => "^" + pattern.Replace("%", ".*").Replace("_", ".") + "$");
                goto case SearchTermMode.Regex;
            }
            case SearchTermMode.Wildcard: {
                patterns = patterns.Select(pattern => "^" + pattern.Replace("*", ".*").Replace("_", ".") + "$");
                goto case SearchTermMode.Regex;
            }
            case SearchTermMode.Regex: {
                return patterns.Any(pattern => Regex.IsMatch(value, pattern, term.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None));
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
        return term.Count switch {
            0 => true,
            1 => Equals(term.Values[0], value),
            _ => term.Values.Contains(value)
        };
    }

    public static MethodInfo GetTermMethodInfo(Type expectedTermType) {
        return Methods
           .FirstOrDefault(m => {
                    var termType = m.GetParameters().First().ParameterType;
                    return expectedTermType.IsGenericTypeDefinition
                        ? termType.IsAssignableToGeneric(expectedTermType)
                        : termType.IsAssignableTo(expectedTermType);
                }
            );
    }
}