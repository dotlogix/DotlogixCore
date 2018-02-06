// ==================================================
// Copyright 2016(C) , DotLogix
// File:  IFixedDuration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  18.07.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities.Options {
    public interface IFixedDuration : IDuration {
        new DateTime UntilUtc { get; set; }
    }
}
