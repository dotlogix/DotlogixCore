// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IEntityContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Infrastructure {
    /// <summary>
    /// An interface to represent a factory to create an <see cref="IEntityContext"/>
    /// </summary>
    public interface IEntityContextFactory {
        /// <summary>
        /// Creates a new instance of <see cref="IEntityContext"/>
        /// </summary>
        IEntityContext Create();
    }
}
