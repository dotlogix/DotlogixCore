// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IApplicationContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Application.Context.Factories {
    /// <summary>
    /// An interface to represent a factory to create <see cref="IApplicationContext"/>
    /// </summary>
    public interface IApplicationContextFactory {
        /// <summary>
        /// Creates a new instance of <see cref="IApplicationContext"/>
        /// </summary>
        IApplicationContext Create();
    }
}
