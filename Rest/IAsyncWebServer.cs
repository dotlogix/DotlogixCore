// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncHttpServer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Rest {
    public interface IAsyncWebServer : IDisposable {
        WebServerSettings Settings { get; }
        bool IsRunning { get; }
        bool IsDisposed { get; }
        void Start();
        void Stop();
        void Dispose();
    }
}
