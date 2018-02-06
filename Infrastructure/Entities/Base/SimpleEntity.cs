// ==================================================
// Copyright 2016(C) , DotLogix
// File:  SimpleEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  24.08.2017
// LastEdited:  06.09.2017
// ==================================================

using System;

namespace DotLogix.Architecture.Infrastructure.Entities.Base {
    public abstract class SimpleEntity : ISimpleEntity {
        public int Id { get; set; }
        public Guid Guid { get; set; }
    }
}
