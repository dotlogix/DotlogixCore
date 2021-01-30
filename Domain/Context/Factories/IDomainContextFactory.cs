// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IDomainContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Domain.Context.Factories {
    /// <summary>
    /// An interface to represent a factory to create a <see cref="IDomainContext"/>
    /// </summary>
    public interface IDomainContextFactory {
        /// <summary>
        /// Creates a new instance of <see cref="IDomainContext"/>
        /// </summary>
        IDomainContext Create();
    }
}
