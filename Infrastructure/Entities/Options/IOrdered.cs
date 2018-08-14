// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IOrdered.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Infrastructure.Entities.Options {
    public interface IOrdered {
        int Order { get; set; }
    }
}
