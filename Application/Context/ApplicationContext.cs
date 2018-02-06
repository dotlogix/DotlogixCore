using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.Services;
using DotLogix.Architecture.Domain.UoW;

namespace DotLogix.Architecture.Application.Context {
    public class ApplicationContext : IApplicationContext
    {
        private readonly IDomainContext _domainContext;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationContext(IDomainContext domainContext, IUnitOfWork unitOfWork)
        {
            this._domainContext = domainContext;
            this._unitOfWork = unitOfWork;
        }

        public TService UseService<TService>() where TService : class, IDomainService
        {
            return this._domainContext.UseService<TService>((IUnitOfWorkContextFactory)this._unitOfWork);
        }
    }
}