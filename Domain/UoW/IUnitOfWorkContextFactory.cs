﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IUnitOfWorkContextFactory.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  07.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Architecture.Domain.UoW {
    public interface IUnitOfWorkContextFactory {
        IUnitOfWorkContext BeginContext();
    }
}
