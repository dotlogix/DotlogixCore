// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InsertOnlyModel.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

using DotLogix.Common.Features;

namespace DotLogix.WebServices.Adapters.Models {
    /// <summary>
    /// A base class for models with soft delete
    /// </summary>
    public abstract class InsertOnlyModelBase : ModelBase, IInsertOnly {
        /// <inheritdoc />
        public bool IsActive { get; set; }
    }
}
