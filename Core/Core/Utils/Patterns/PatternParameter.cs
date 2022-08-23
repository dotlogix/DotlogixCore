namespace DotLogix.Core.Utils.Patterns; 

public class PatternParameter {
    public IRegexPatternType Type { get; set; }
    public int Offset { get; set; }
    public int Count { get; set; }
        
    public string Name { get; set; }
    public string Variant { get; set; }
    public string[] Args { get; set; }
        
    public string Regex { get; set; }
}