﻿// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IApplicationContext.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using DotLogix.Architecture.Domain.Services;
#endregion

namespace DotLogix.Architecture.Application.Context {
    public interface IApplicationContext {
        TService UseService<TService>() where TService : class, IDomainService;
    }
}
