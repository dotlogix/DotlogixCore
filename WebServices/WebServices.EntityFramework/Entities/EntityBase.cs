// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SimpleEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Common.Features;
#endregion

namespace DotLogix.WebServices.EntityFramework.Entities {
    /// <summary>
    /// A base class for entities
    /// </summary>
    public abstract class EntityBase : IIdentity, IGuid {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public Guid Guid { get; set; }
    }
}
