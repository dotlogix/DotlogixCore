// ==================================================
// Copyright 2018(C) , DotLogix
// File:  InsertOnlyEntity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Common.Entities.Base {
    public abstract class InsertOnlyEntity : SimpleEntity, IInsertOnlyEntity {
        public bool IsActive { get; set; }
    }
}
