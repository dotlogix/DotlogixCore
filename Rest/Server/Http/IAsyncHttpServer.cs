// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncHttpServer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
