// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MimeTypes.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
#endregion

namespace DotLogix.Core.Rest.Server.Http.Mime {
    public static class MimeTypes {
        public static MimeType PlainText { get; } = new MimeType("text/plain");
        public static MimeType Json { get; } = new MimeType("application/json");
    }
}
