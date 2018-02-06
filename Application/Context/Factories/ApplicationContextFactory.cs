using DotLogix.Architecture.Domain.Context.Factories;
using DotLogix.Architecture.Infrastructure.UoW;

namespace DotLogix.Architecture.Application.Context.Factories
{
    public class ApplicationContextFactory : IApplicationContextFactory
    {
        private readonly IDomainContextFactory _domainContextFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ApplicationContextFactory(IDomainContextFactory domainContextFactory, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this._domainContextFactory = domainContextFactory;
            this._unitOfWorkFactory = unitOfWorkFactory;
        }

        public IApplicationContext Create()
        {
            return (IApplicationContext)new ApplicationContext(this._domainContextFactory.Create(), this._unitOfWorkFactory.Create());
        }
    }
}