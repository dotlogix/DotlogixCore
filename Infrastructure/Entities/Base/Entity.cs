// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Entity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Infrastructure.Entities.Base {
    /// <summary>
    /// A simple entity implementing the <see cref="IEntity"/> interface
    /// </summary>
    public abstract class Entity : InsertOnlyEntity, IEntity {
        /// <inheritdoc />
        public int Order { get; set; }
    }
}
