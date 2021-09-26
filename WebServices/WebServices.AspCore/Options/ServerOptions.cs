// ==================================================
// Copyright 2019(C) , DotLogix
// File:  ServerConfiguration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  12.01.2019
// LastEdited:  20.01.2019
// ==================================================

namespace DotLogix.WebServices.AspCore.Options {
    public class ServerOptions {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 1337;
    }
}
