// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SimpleEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities.Base {
    public abstract class SimpleEntity : ISimpleEntity {
        public int Id { get; set; }
        public Guid Guid { get; set; }
    }
}
