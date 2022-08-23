using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Common.Features;

namespace DotLogix.WebServices.AspCore.Services; 

public interface IDomainService { }
    
public interface IDomainService<TResponse, TCreate, TEnsure, TPatch> : IReadOnlyDomainService<TResponse>
    where TResponse : class, IGuid
    where TCreate : class, IGuid
    where TEnsure : class, IGuid
    where TPatch : class, IGuid {

    #region Create
    public Task<TResponse> CreateAsync(TCreate request);
    public Task<ICollection<TResponse>> CreateRangeAsync(IEnumerable<TCreate> requests);
    #endregion

    #region Patch
    public Task<TResponse> PatchAsync(TPatch request);
    public Task<ICollection<TResponse>> PatchRangeAsync(IEnumerable<TPatch> requests);
    #endregion

    #region Ensure
    public Task<TResponse> EnsureAsync(TEnsure requests);
    public Task<ICollection<TResponse>> EnsureRangeAsync(IEnumerable<TEnsure> requests);
    #endregion

    #region Remove
    public Task<bool> RemoveAsync(Guid guid);
    public Task<int> RemoveRangeAsync(IEnumerable<Guid> guids);
    #endregion
}
    
public interface IDomainService<TResponse, TCreate, TEnsure, TPatch, TFilter> : IDomainService<TResponse, TCreate, TEnsure, TPatch>, IReadOnlyDomainService<TResponse, TFilter>
    where TResponse : class, IGuid
    where TCreate : class, IGuid
    where TEnsure : class, IGuid
    where TPatch : class, IGuid {
}