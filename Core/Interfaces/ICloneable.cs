// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ICloneable.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  07.07.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Interfaces {
    public interface ICloneable<out T> : ICloneable {
        new T Clone();
    }
}
