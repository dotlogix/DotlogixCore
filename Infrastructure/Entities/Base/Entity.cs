// ==================================================
// Copyright 2016(C) , DotLogix
// File:  Entity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  24.08.2017
// LastEdited:  06.09.2017
// ==================================================

namespace DotLogix.Architecture.Infrastructure.Entities.Base {
    public abstract class Entity : InsertOnlyEntity, IEntity {
        public int Order { get; set; }
    }
}
