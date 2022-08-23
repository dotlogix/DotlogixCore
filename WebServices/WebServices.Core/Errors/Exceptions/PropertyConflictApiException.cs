using System.Collections.Generic;

namespace DotLogix.WebServices.Core.Errors; 

public class PropertyConflictApiException : ConflictApiException {
    public string SourceProperty { get; set; }
    public string TargetProperty { get; set; }

    public PropertyConflictApiException(string message = null)
        : base(ApiErrorKinds.PropertyConflict, message) {
            
    }
        
    protected override void AppendContext(IDictionary<string, object> dictionary) {
        base.AppendContext(dictionary);
        if(SourceProperty is not null) {
            dictionary.Add("SourceProperty", SourceProperty);
        }
        if(TargetProperty is not null) {
            dictionary.Add("TargetProperty", TargetProperty);
        }
    }
}