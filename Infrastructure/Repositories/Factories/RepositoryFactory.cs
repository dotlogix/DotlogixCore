// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RepositoryFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Architecture.Infrastructure.EntityContext;
#endregion

namespace DotLogix.Architecture.Infrastructure.Repositories.Factories {
    /// <summary>
    /// A <see cref="IRepositoryFactory"/> using a callback method to create instances
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory {
        private readonly Func<IEntitySetProvider, IRepository> _createRepositoryFunc;

        /// <summary>
        /// Creates a new instance of <see cref="RepositoryFactory"/>
        /// </summary>
        public RepositoryFactory(Func<IEntitySetProvider, IRepository> createRepositoryFunc) {
            _createRepositoryFunc = createRepositoryFunc;
        }

        /// <inheritdoc />
        public IRepository Create(IEntitySetProvider entitySetProvider) {
            return _createRepositoryFunc.Invoke(entitySetProvider);
        }
    }
}
