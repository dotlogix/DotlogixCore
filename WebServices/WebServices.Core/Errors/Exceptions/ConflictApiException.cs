using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using DotLogix.Core.Extensions;

namespace DotLogix.WebServices.Core.Errors; 

public class ConflictApiException : TypedApiException {
    private Type _existingClrType;
    private Type _conflictClrType;

    public ConflictApiException(string message = null)
        : base(ApiErrorKinds.Conflict, HttpStatusCode.Conflict) {
        Message = message;
    }
    protected ConflictApiException(string kind, string message = null)
        : base(kind, HttpStatusCode.Conflict) {
        Message = message;
    }
        
    public Type ExistingClrType {
        get => _existingClrType ?? ExistingObject?.GetType();
        set => _existingClrType = value;
    }
    public Type ConflictClrType {
        get => _conflictClrType ?? ExistingObject?.GetType();
        set => _conflictClrType = value;
    }
    public object ExistingObject { get; set; }
    public object ConflictObject { get; set; }

    protected override string GetErrorMessage() {
        if(Message is not null) {
            return Message;
        }
            
        var existingType = ExistingClrType;
        var conflictType = ConflictClrType;
            
        var sb = new StringBuilder("Found conflict between objects");
        if(existingType is not null && conflictType is not null) {
            sb.Append($" of type {existingType.GetFriendlyName()}");
            if(existingType != conflictType) {
                sb.Append($" and {conflictType.GetFriendlyName()}");
            }
        } else if (existingType is not null || conflictType is not null) {
            sb.Append($" of type {(existingType ?? conflictType).GetFriendlyName()}");
        }
        return sb.ToString();
    }

    protected override void AppendContext(IDictionary<string, object> dictionary) {
        base.AppendContext(dictionary);
        if(ExistingClrType is not null) {
            dictionary.Add("ExistingType", ExistingClrType.GetFriendlyName());
        }
        if(ConflictClrType is not null) {
            dictionary.Add("ConflictType", ConflictClrType.GetFriendlyName());
        }
        if(ExistingObject is not null) {
            dictionary.Add("Existing", ExistingObject);
        }
        if(ConflictObject is not null) {
            dictionary.Add("Conflict", ConflictObject);
        }
    }
}