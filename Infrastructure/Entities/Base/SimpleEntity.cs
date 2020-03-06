// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SimpleEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities.Base {
    /// <summary>
    /// A simple entity implementing the <see cref="ISimpleEntity"/> interface
    /// </summary>
    public abstract class SimpleEntity : ISimpleEntity {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public Guid Guid { get; set; }
    }
}
