// ==================================================
// Copyright 2018(C) , DotLogix
// File:  DynamicRepositoryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Architecture.Infrastructure.EntityContext;
using DotLogix.Core.Extensions;
using DotLogix.Core.Reflection.Dynamics;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories.Factories {
    public class DynamicRepositoryFactory : IRepositoryFactory {
        private readonly DynamicCtor _repoCtor;

        public DynamicRepositoryFactory(DynamicCtor repoCtor) {
            _repoCtor = repoCtor;
        }

        public IRepository Create(IEntitySetProvider entitySetProvider) {
            return (IRepository)_repoCtor.Invoke(entitySetProvider);
        }

        public static IRepositoryFactory CreateFor<TRepo>() where TRepo : IRepository {
            return CreateFor(typeof(TRepo));
        }

        public static IRepositoryFactory CreateFor(Type repoType) {
            if(repoType.IsAssignableTo(typeof(IRepository)) == false)
                throw new ArgumentException($"Type {repoType} is not assignable to type {nameof(IRepository)}", nameof(repoType));

            var repoCtor = repoType.GetConstructor(new[] {typeof(IEntitySetProvider)});
            if(repoCtor == null)
                throw new ArgumentException($"Type {repoType} doesnt have a valid constructor [.ctor({typeof(IEntitySetProvider).Name})]", nameof(repoType));
            var dynamicCtor = repoCtor.CreateDynamicCtor();
            if(dynamicCtor == null)
                throw new ArgumentException($"Can not create dynamic constructor for type {repoType}");
            return new DynamicRepositoryFactory(dynamicCtor);
        }
    }
}
