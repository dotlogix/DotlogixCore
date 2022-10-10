// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NamedModel.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Common.Options;
#endregion

namespace DotLogix.Architecture.Domain.Models.Base {
    /// <summary>
    /// An implementation of the <see cref="IModel"/> and the <see cref="INamed"/> interface
    /// </summary>
    public abstract class NamedModel : Model, INamed {
        /// <inheritdoc />
        public string Name { get; set; }
    }
}
