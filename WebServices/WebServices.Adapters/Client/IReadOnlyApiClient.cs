using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotLogix.Common.Features;

namespace DotLogix.WebServices.Adapters.Client; 

public interface IReadOnlyApiClient<TResponse, in TFilter>
    where TResponse : IGuid
{
    Task<TResponse> GetAsync(Guid guid, CancellationToken cancellationToken = default);
    Task<ICollection<TResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ICollection<TResponse>> GetRangeAsync(IEnumerable<Guid> guids, CancellationToken cancellationToken = default);
    Task<ICollection<TResponse>> FilterAsync(TFilter filter, CancellationToken cancellationToken = default);
}