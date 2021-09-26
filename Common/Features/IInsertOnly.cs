// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IInsertOnly.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Common.Features {
    /// <summary>
    /// An interface to represent insert only objects
    /// </summary>
    public interface IInsertOnly {
        /// <summary>
        /// A value to check if this object is active or not
        /// </summary>
        bool IsActive { get; set; }
    }
}
