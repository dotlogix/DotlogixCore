using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace DotLogix.WebServices.Core.Errors; 

public class ApiError {
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }
    public string Kind { get; set; }
    public string Message { get; set; }
        
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto, DefaultValueHandling = DefaultValueHandling.Ignore)]
    public IReadOnlyDictionary<string, object> Context { get; set; }
}