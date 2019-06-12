// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InsertOnlyEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Infrastructure.Entities.Base {
    /// <summary>
    /// A simple entity implementing the <see cref="ISimpleEntity"/> and the <see cref="IInsertOnlyEntity"/> interface
    /// </summary>
    public abstract class InsertOnlyEntity : SimpleEntity, IInsertOnlyEntity {
        /// <inheritdoc />
        public bool IsActive { get; set; }
    }
}
