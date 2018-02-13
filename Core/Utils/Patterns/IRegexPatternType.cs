namespace DotLogix.Core.Utils.Patterns {
    public interface IRegexPatternType {
        string Name { get; }
        string GetRegexPattern(string variant, string[] args);
    }
}