// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InsertOnlyEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

using DotLogix.Common.Features;

namespace DotLogix.WebServices.EntityFramework.Entities {
    /// <summary>
    /// A base class for entities with soft delete
    /// </summary>
    public abstract class InsertOnlyEntityBase : EntityBase, IInsertOnly {
        /// <inheritdoc />
        public bool IsActive { get; set; }
    }
}
