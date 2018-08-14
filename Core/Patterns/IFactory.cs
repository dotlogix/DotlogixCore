// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

namespace DotLogix.Core.Patterns {
    public interface IFactory<out T> {
        T Create();
    }
}
