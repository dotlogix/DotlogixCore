// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IApplicationContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Architecture.Application.Context.Factories {
    public interface IApplicationContextFactory {
        IApplicationContext Create();
    }
}
