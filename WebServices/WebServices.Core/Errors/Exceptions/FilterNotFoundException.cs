using System;
using System.Collections.Generic;
using System.Net;
using DotLogix.Core.Extensions;

namespace DotLogix.WebServices.Core.Errors; 

public class FilterNotFoundException : TypedApiException {
    private Type _filterClrType;
    public object Filter { get; set; }

    public Type FilterClrType {
        get => _filterClrType ?? Filter?.GetType();
        set => _filterClrType = value;
    }

    public FilterNotFoundException(object filter = null)
        : base(ApiErrorKinds.FilterNotFound, HttpStatusCode.NotFound) {
        Filter = filter;
    }

    protected override string GetErrorMessage() {
        return $"Can not find entity of type {ClrType.GetFriendlyName()} matching the provided filter of type {FilterClrType.GetFriendlyName()}";
    }

    protected override void AppendContext(IDictionary<string, object> dictionary) {
        base.AppendContext(dictionary);
        if(FilterClrType is not null) {
            dictionary.Add("FilterType", FilterClrType.GetFriendlyName());
        }
        if(Filter is not null) {
            dictionary.Add("Filter", Filter);
        }
    }
}