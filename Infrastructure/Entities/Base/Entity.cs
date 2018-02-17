// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Entity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Architecture.Infrastructure.Entities.Base {
    public abstract class Entity : InsertOnlyEntity, IEntity {
        public int Order { get; set; }
    }
}
