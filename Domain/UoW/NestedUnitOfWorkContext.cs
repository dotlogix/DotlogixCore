// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NestedUnitOfWorkContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Repositories;
#endregion

namespace DotLogix.Architecture.Domain.UoW {
    /// <summary>
    /// A wrapper class for a nested <see cref="IUnitOfWorkContext"/>
    /// </summary>
    public class NestedUnitOfWorkContext : IUnitOfWorkContext {
        /// <summary>
        /// The parent context
        /// </summary>
        protected IUnitOfWorkContext ParentContext { get; }
        /// <summary>
        /// Create a new instance of <see cref="NestedUnitOfWorkContext"/>
        /// </summary>
        public NestedUnitOfWorkContext(IUnitOfWorkContext parentContext) {
            ParentContext = parentContext;
        }

        /// <inheritdoc />
        public IUnitOfWorkContext BeginContext() {
            return ParentContext.BeginContext();
        }

        /// <inheritdoc />
        public TRepo UseRepository<TRepo>() where TRepo : IRepository {
            return ParentContext.UseRepository<TRepo>();
        }

        /// <inheritdoc />
        public Task CompleteAsync() {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose() { }
    }
}
