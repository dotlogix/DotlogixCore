// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IGuid.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Architecture.Infrastructure.Entities.Options {
    public interface IGuid {
        Guid Guid { get; set; }
    }
}
