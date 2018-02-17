// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Patterns {
    public interface IFactory<out T> {
        T Create();
    }
}
