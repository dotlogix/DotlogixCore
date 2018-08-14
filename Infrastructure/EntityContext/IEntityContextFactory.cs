// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntityContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Infrastructure.EntityContext {
    public interface IEntityContextFactory {
        IEntityContext Create();
    }
}
