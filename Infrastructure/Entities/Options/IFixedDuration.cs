// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IFixedDuration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities.Options {
    public interface IFixedDuration : IDuration {
        new DateTime UntilUtc { get; set; }
    }
}
