// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public interface IEntityContext : IDisposable {
        Task CompleteAsync();
    }
}
