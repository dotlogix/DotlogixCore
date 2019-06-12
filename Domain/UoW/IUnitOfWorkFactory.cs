// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IUnitOfWorkFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Domain.UoW {
    /// <summary>
    /// An interface to represent a factory to create a <see cref="IUnitOfWork"/>
    /// </summary>
    public interface IUnitOfWorkFactory {
        /// <summary>
        /// Creates a new instance of <see cref="IUnitOfWork"/>
        /// </summary>
        IUnitOfWork Create();
    }
}
