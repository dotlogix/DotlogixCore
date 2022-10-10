// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InsertOnlyModel.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Domain.Models.Base {
    /// <summary>
    /// An implementation of the <see cref="IInsertOnlyModel"/> interface
    /// </summary>
    public abstract class InsertOnlyModel : SimpleModel, IInsertOnlyModel {
        /// <inheritdoc />
        public bool IsActive { get; set; }
    }
}
