// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IFixedDuration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Architecture.Common.Options {
    public interface IFixedDuration : IDuration {
        new DateTime UntilUtc { get; set; }
    }
}
