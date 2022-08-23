// ==================================================
// Copyright 2014-2022(C), DotLogix
// File:  IBuilder.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created: 24.07.2022 01:13
// LastEdited:  24.07.2022 01:13
// ==================================================

namespace DotLogix.Core.Utils.Builders; 

public interface IBuilder<out T> {
    T Build();
}