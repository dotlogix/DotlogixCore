// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IInsertOnly.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Architecture.Infrastructure.Entities.Options {
    public interface IInsertOnly {
        bool IsActive { get; set; }
    }
}
