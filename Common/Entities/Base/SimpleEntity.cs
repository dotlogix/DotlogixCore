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

namespace DotLogix.Architecture.Common.Entities.Base {
    public abstract class SimpleEntity : ISimpleEntity {
        public int Id { get; set; }
        public Guid Guid { get; set; }
    }
}
