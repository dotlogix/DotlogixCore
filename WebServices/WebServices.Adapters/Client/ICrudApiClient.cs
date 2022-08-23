using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Common.Features;

namespace DotLogix.WebServices.Adapters.Client; 

public interface ICrudApiClient<TResponse, in TEnsure, in TFilter>
    : ICrudApiClient<TResponse, TEnsure, TEnsure, TEnsure, TFilter>
    where TResponse : IGuid
    where TEnsure : IGuid
{
}

public interface ICrudApiClient<TResponse, in TCreate, in TEnsure, in TPatch, in TFilter>
    : IReadOnlyApiClient<TResponse, TFilter>
    where TResponse : IGuid
    where TCreate : IGuid
    where TEnsure : IGuid
    where TPatch : IGuid
{
    Task<TResponse> CreateAsync(TCreate request, CancellationToken cancellationToken = default);
    Task<ICollection<TResponse>> CreateRangeAsync(IEnumerable<TCreate> requests, CancellationToken cancellationToken = default);

    Task<TResponse> EnsureAsync(TEnsure request, CancellationToken cancellationToken = default);
    Task<ICollection<TResponse>> EnsureRangeAsync(IEnumerable<TEnsure> requests, CancellationToken cancellationToken = default);

    Task<TResponse> PatchAsync(TPatch request, CancellationToken cancellationToken = default);
    Task<ICollection<TResponse>> PatchRangeAsync(IEnumerable<TPatch> requests, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<int> DeleteRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default);
}