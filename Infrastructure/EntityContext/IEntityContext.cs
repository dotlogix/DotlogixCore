// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntityContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  19.03.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Threading.Tasks;
#endregion

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public interface IEntityContext : IEntitySetProvider, IDisposable {
        Task CompleteAsync();
    }
}
