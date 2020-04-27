using System;

namespace DotLogix.Core.Rest.Services {
    public interface IWebServiceObjectResult : IWebServiceResult {
        Type ReturnType { get; }
        Optional<object> ReturnValue { get; }
    }

    public interface IWebServiceObjectResult<T> : IWebServiceObjectResult {
        new Optional<T> ReturnValue { get; }
    }
}