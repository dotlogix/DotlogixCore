// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ITitled.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Architecture.Common.Options {
    /// <summary>
    /// An interface to force a title
    /// </summary>
    public interface ITitled {
        /// <summary>
        /// The title
        /// </summary>
        string Title { get; set; }
    }
}
