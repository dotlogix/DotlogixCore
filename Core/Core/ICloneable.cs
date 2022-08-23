// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ICloneable.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core {
    /// <summary>
    /// A typed cloneable interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneable<out T> : ICloneable {
        /// <summary>
        /// Clone object
        /// </summary>
        /// <returns></returns>
        new T Clone();
    }
}