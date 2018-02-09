using DotLogix.Architecture.Domain.Context;
using DotLogix.Architecture.Domain.Services;
using DotLogix.Architecture.Domain.UoW;

namespace DotLogix.Architecture.Application.Context {
    public class ApplicationContext : IApplicationContext
    {
        protected IDomainContext DomainContext { get; }
        protected IUnitOfWork UnitOfWork { get; }

        public ApplicationContext(IDomainContext domainContext, IUnitOfWork unitOfWork)
        {
            DomainContext = domainContext;
            UnitOfWork = unitOfWork;
        }

        public TService UseService<TService>() where TService : class, IDomainService
        {
            return DomainContext.UseService<TService>(UnitOfWork);
        }
    }
}