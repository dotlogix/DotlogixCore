using System;
using DotLogix.Architecture.Infrastructure.EntityContext;

namespace DotLogix.Architecture.Infrastructure.Repositories.Factories {
    public class RepositoryFactory : IRepositoryFactory {
        private readonly Func<IEntityContext, IRepository> _createRepositoryFunc;
        public RepositoryFactory(Func<IEntityContext, IRepository> createRepositoryFunc) {
            _createRepositoryFunc = createRepositoryFunc;
        }

        public IRepository Create(IEntityContext entitySetProvider) {
            return _createRepositoryFunc.Invoke(entitySetProvider);
        }
    }
}