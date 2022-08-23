#region
using System;
using System.Text.RegularExpressions;
using DotLogix.WebServices.Core.Serialization;
using Newtonsoft.Json;
#endregion

namespace DotLogix.WebServices.Core.Terms; 

[JsonConverter(typeof(SearchTermJsonConverter))]
public class SearchTerm {
    public ManyTerm<string> Pattern { get; set; }
    public SearchTermMode Mode { get; set; }
    public double? FuzzyThreshold { get; set; }
    public bool IgnoreCase { get; set; }

    private StringComparison StringComparison => IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
    private RegexOptions RegexOptions => IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;

    public static SearchTerm EqualTo(ManyTerm<string> pattern, bool ignoreCase = false) {
        return new SearchTerm {Pattern = pattern, IgnoreCase = ignoreCase, Mode = SearchTermMode.Equals};
    }

    public static SearchTerm StartsWith(ManyTerm<string> pattern, bool ignoreCase = false) {
        return new SearchTerm {Pattern = pattern, IgnoreCase = ignoreCase, Mode = SearchTermMode.StartsWith};
    }

    public static SearchTerm Contains(ManyTerm<string> pattern, bool ignoreCase = false) {
        return new SearchTerm {Pattern = pattern, IgnoreCase = ignoreCase, Mode = SearchTermMode.Contains};
    }

    public static SearchTerm EndsWith(ManyTerm<string> pattern, bool ignoreCase = false) {
        return new SearchTerm {Pattern = pattern, IgnoreCase = ignoreCase, Mode = SearchTermMode.EndsWith};
    }

    public static SearchTerm Regex(ManyTerm<string> pattern, bool ignoreCase = false) {
        return new SearchTerm {Pattern = pattern, IgnoreCase = ignoreCase, Mode = SearchTermMode.Regex};
    }

    public static SearchTerm Like(ManyTerm<string> pattern, bool ignoreCase = false) {
        return new SearchTerm {Pattern = pattern, IgnoreCase = ignoreCase, Mode = SearchTermMode.Like};
    }
        
    public static SearchTerm Wildcard(ManyTerm<string> pattern, bool ignoreCase = false) {
        return new SearchTerm {Pattern = pattern, IgnoreCase = ignoreCase, Mode = SearchTermMode.Wildcard};
    }

    public static SearchTerm Fuzzy(ManyTerm<string> pattern, double fuzzyThreshold) {
        return new SearchTerm {Pattern = pattern, IgnoreCase = true, Mode = SearchTermMode.Fuzzy, FuzzyThreshold = fuzzyThreshold};
    }

    public static implicit operator SearchTerm(string value) {
        return EqualTo(value);
    }
        
    public static implicit operator SearchTerm(ManyTerm<string> value) {
        return EqualTo(value);
    }
}