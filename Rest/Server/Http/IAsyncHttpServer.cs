// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncHttpServer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  29.01.2018
// LastEdited:  31.01.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest.Server.Http {
    public interface IAsyncHttpServer : IDisposable {
        ConcurrencyOptions ConcurrencyOptions { get; }
        bool IsRunning { get; }
        void Start();
        void Stop();
        void AddServerPrefix(string uriPrefix);
    }
}
