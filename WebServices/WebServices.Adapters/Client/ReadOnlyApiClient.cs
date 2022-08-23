using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.WebServices.Adapters.Endpoints;
using DotLogix.WebServices.Adapters.Http;

namespace DotLogix.WebServices.Adapters.Client; 

public abstract class ReadOnlyApiClient<TResponse, TFilter> : ErrorHandlingApiClient, IReadOnlyApiClient<TResponse, TFilter>
    where TResponse : IGuid
{
    protected ReadOnlyApiClient(IHttpProvider httpProvider, IWebServiceEndpoint endpoint, string relativeUri)
        : base(httpProvider, endpoint, relativeUri)
    {
    }

    public virtual Task<TResponse> GetAsync(Guid guid, CancellationToken cancellationToken = default)
    {
        return GetAsync<TResponse>($"{guid:D}", cancellationToken);
    }

    public virtual Task<ICollection<TResponse>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default)
    {
        return PostAsync<ICollection<TResponse>>(guids, "filtered/range", cancellationToken);
    }

    public virtual Task<ICollection<TResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return GetAsync<ICollection<TResponse>>(cancellationToken: cancellationToken);
    }

    public virtual Task<ICollection<TResponse>> FilterAsync(TFilter filter, CancellationToken cancellationToken = default)
    {
        return PostAsync<ICollection<TResponse>>(filter, "filtered", cancellationToken);
    }
}