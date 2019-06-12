// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SimpleModel.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Architecture.Domain.Models.Base {
    /// <summary>
    /// An implementation of the <see cref="ISimpleModel"/> interface
    /// </summary>
    public abstract class SimpleModel : ISimpleModel {
        /// <inheritdoc />
        public Guid Guid { get; set; }
    }
}
