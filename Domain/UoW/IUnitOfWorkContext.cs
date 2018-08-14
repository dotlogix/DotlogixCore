// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IUnitOfWorkContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
using DotLogix.Architecture.Infrastructure.Repositories;
#endregion

namespace DotLogix.Architecture.Domain.UoW {
    public interface IUnitOfWorkContext : IUnitOfWorkContextFactory, IDisposable {
        TRepo UseRepository<TRepo>() where TRepo : IRepository;
        Task CompleteAsync();
    }
}
