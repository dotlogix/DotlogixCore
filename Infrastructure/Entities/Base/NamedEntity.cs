// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NamedEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using DotLogix.Architecture.Common.Options;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities.Base {
    /// <summary>
    /// A simple entity implementing the <see cref="IEntity"/> and the <see cref="INamed"/> interface
    /// </summary>
    public abstract class NamedEntity : Entity, INamed {
        /// <inheritdoc />
        public string Name { get; set; }
    }
}
