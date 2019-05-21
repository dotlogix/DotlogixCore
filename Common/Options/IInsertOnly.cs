// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IInsertOnly.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Common.Options {
    public interface IInsertOnly {
        bool IsActive { get; set; }
    }
}
