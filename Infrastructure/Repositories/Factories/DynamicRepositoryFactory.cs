using System;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;

namespace DotLogix.Architecture.Infrastructure.Repositories.Factories {
    public class DynamicRepositoryFactory : IRepositoryFactory {
        private readonly DynamicCtor _repoCtor;
        private readonly Type _contextType;

        public DynamicRepositoryFactory(DynamicCtor repoCtor, Type contextType) {
            _repoCtor = repoCtor;
            _contextType = contextType;
        }

        public IRepository Create(IEntityContext entityContext) {
            if(_contextType.IsInstanceOfType(entityContext)==false)
                throw new ArgumentException($"Context has to be type of {_contextType.Name}", nameof(entityContext));
            return (IRepository)_repoCtor.Invoke(entityContext);
        }

        public static IRepositoryFactory CreateFor<TRepo, TEntityContext>() where TRepo : IRepository where TEntityContext : IEntityContext {
            return CreateFor(typeof(TRepo), typeof(TEntityContext));
        }

        public static IRepositoryFactory CreateFor(Type repoType, Type contextType) {
            if (repoType.IsAssignableTo(typeof(IRepository)) == false)
                throw new ArgumentException($"Type {repoType} is not assignable to type {nameof(IRepository)}", nameof(repoType));

            var repoCtor = repoType.GetConstructor(new[] { contextType });
            if (repoCtor == null)
                throw new ArgumentException($"Type {repoType} doesnt have a valid constructor [.ctor({contextType.Name})]", nameof(repoType));
            var dynamicCtor = repoCtor.CreateDynamicCtor();
            if(dynamicCtor == null)
                throw new ArgumentException($"Can not create dynamic constructor for type {repoType}");
            return new DynamicRepositoryFactory(dynamicCtor, contextType);
        }
    }

    
}