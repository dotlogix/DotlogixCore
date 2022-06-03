// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IGuid.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Common.Features {
    /// <summary>
    /// An interface to force a guid key
    /// </summary>
    public interface IGuid {
        /// <summary>
        /// The guid
        /// </summary>
        Guid Guid { get; set; }
    }
}
