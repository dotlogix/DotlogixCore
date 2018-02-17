// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NestedUnitOfWorkContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  13.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Repositories;
#endregion

namespace DotLogix.Architecture.Domain.UoW {
    public class NestedUnitOfWorkContext : IUnitOfWorkContext {
        protected IUnitOfWorkContext ParentContext { get; }

        public NestedUnitOfWorkContext(IUnitOfWorkContext parentContext) {
            ParentContext = parentContext;
        }

        public IUnitOfWorkContext BeginContext() {
            return ParentContext.BeginContext();
        }

        public TRepo UseRepository<TRepo>() where TRepo : IRepository {
            return ParentContext.UseRepository<TRepo>();
        }

        public Task CompleteAsync() {
            return Task.CompletedTask;
        }

        public void Dispose() { }
    }
}
