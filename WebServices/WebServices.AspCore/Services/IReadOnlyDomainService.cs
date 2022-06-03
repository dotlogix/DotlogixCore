using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotLogix.Common.Features;
using DotLogix.WebServices.Core.Terms;

namespace DotLogix.WebServices.AspCore.Services {
    public interface IReadOnlyDomainService<TResponse> : IDomainService
        where TResponse : class, IGuid {
        #region Get
        public Task<TResponse> GetAsync(Guid guid);
        public Task<ICollection<TResponse>> GetRangeAsync(IEnumerable<Guid> guids);
        public Task<ICollection<TResponse>> GetAllAsync();
        #endregion
    }
    
    public interface IReadOnlyDomainService<TResponse, TFilter> : IReadOnlyDomainService<TResponse>
        where TResponse : class, IGuid {
        #region Get
        public Task<ICollection<TResponse>> GetFilteredAsync(TFilter filter);
        Task<PaginationResult<TResponse>> GetPagedAsync(PaginationTerm paginationTerm);
        Task<PaginationResult<TResponse>> GetPagedAsync(TFilter filter, PaginationTerm paginationTerm);
        #endregion
    }
}
