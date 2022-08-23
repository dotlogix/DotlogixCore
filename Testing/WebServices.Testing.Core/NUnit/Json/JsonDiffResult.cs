#region using
using Newtonsoft.Json.Linq;
#endregion

namespace DotLogix.WebServices.Testing.NUnit.Json; 

public class JsonDiffResult
{
    public bool HasDifferences { get; set; }

    public JToken Expected { get; set; }
    public JToken Actual { get; set; }

    public JToken ExpectedDifferences { get; set; }
    public JToken ActualDifferences { get; set; }

    public JsonDiffOptions Options { get; set; }
}