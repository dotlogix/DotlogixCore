// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InsertOnlyEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

using DotLogix.Architecture.Common.Options;

namespace DotLogix.Architecture.Infrastructure.Entities.Base {
    /// <summary>
    /// A simple entity implementing the <see cref="ISimpleEntity"/> and the <see cref="IInsertOnly"/> interface
    /// </summary>
    public abstract class InsertOnlyEntity : SimpleEntity, IInsertOnly {
        /// <inheritdoc />
        public bool IsActive { get; set; }
    }
}
