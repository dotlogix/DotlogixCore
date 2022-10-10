// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IUnitOfWorkContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Domain.UoW {
    /// <summary>
    /// An interface to represent a factory to create a <see cref="IUnitOfWorkContext"/>
    /// </summary>
    public interface IUnitOfWorkContextFactory {
        /// <summary>
        /// Creates a new <see cref="IUnitOfWorkContext"/>
        /// </summary>
        IUnitOfWorkContext BeginContext();
    }
}
