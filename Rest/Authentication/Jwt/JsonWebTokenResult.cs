// ==================================================
// Copyright 2019(C) , DotLogix
// File:  JsonWebTokenResult.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  03.01.2019
// LastEdited:  03.01.2019
// ==================================================

namespace DotLogix.Core.Rest.Authentication.Jwt {
    public enum JsonWebTokenResult {
        Success,
        InvalidFormat,
        InvalidType,
        InvalidAlgorithm,
        InvalidSignature
    }
}
