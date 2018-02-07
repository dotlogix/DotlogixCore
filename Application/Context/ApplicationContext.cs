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
            _domainContext = domainContext;
            _unitOfWork = unitOfWork;
        }

        public TService UseService<TService>() where TService : class, IDomainService
        {
            return _domainContext.UseService<TService>(_unitOfWork);
        }
    }
}