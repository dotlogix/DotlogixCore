// ==================================================
// Copyright 2019(C) , DotLogix
// File:  SecurityConfiguration.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  28.08.2018
// LastEdited:  20.01.2019
// ==================================================

#region
#endregion

namespace DotLogix.WebServices.Authentication.Options {
    public class AuthOptions {
        public string JwtAudience { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtSigningKey { get; set; }
    }
}
