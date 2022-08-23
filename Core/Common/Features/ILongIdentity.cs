// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  ILongIdentity.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 25.06.2022 03:42
// LastEdited:  25.06.2022 03:42
// ==================================================

namespace DotLogix.Common.Features {
    /// <summary>
    ///     An interface to force an id
    /// </summary>
    public interface ILongIdentity {
        /// <summary>
        ///     The id
        /// </summary>
        long Id { get; set; }
    }
}
