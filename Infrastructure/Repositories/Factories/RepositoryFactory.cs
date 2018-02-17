// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RepositoryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using DotLogix.Architecture.Infrastructure.EntityContext;
#endregion

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
