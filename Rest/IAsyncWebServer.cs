// ==================================================
// Copyright 2018(C) , DotLogix
// File:  IAsyncHttpServer.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using DotLogix.Core.Rest.Http;
#endregion

namespace DotLogix.Core.Rest {
    public interface IAsyncWebServer : IDisposable {
        WebServerConfiguration Configuration { get; }
        bool IsRunning { get; }
        void Start();
        void Stop();
        void AddServerPrefix(string uriPrefix);
    }
}
