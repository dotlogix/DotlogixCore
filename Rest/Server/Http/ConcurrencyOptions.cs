// ==================================================
// Copyright 2018(C) , DotLogix
// File:  ConcurrencyOptions.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

namespace DotLogix.Core.Rest.Server.Http {
    public class ConcurrencyOptions {
        public static ConcurrencyOptions Default => new ConcurrencyOptions(64, 64);
        public int Connections { get; }
        public int Requests { get; }

        public ConcurrencyOptions(int connections, int requests) {
            Connections = connections;
            Requests = requests;
        }
    }
}
