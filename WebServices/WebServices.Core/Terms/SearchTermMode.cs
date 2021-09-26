namespace DotLogix.WebServices.Core.Terms {
    public enum SearchTermMode
    {
        Equals,
        StartsWith,
        Contains,
        EndsWith,

        Like,
        Wildcard,
        
        Regex,
        Fuzzy,
    }
}
