// ==================================================
// Copyright 2016(C) , DotLogix
// File:  IInsertOnly.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  18.07.2017
// LastEdited:  06.09.2017
// ==================================================

namespace DotLogix.Architecture.Infrastructure.Entities.Options {
    public interface IInsertOnly {
        bool IsActive { get; set; }
    }
}
