// ==================================================
// Copyright 2016(C) , DotLogix
// File:  IDuration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  18.07.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities.Options {
    public interface IDuration {
        DateTime FromUtc { get; set; }
        DateTime? UntilUtc { get; set; }
    }
}
