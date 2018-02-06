// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IWebService.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

namespace DotLogix.Core.Rest.Services {
    public interface IWebService {
        string Name { get; }
        string RoutePrefix { get; }
    }
}
