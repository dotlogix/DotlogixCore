// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MimeType.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
#endregion

#region
using System.Collections.Generic;
#endregion

namespace DotLogix.Core.Rest.Server.Http.Headers {
    public class MimeType : HeaderValue {
        public MimeType(string value, IDictionary<string, Optional<string>> attributes = null) : base(value, attributes) { }

        public static MimeType Parse(string value) {
            if(value == null)
                return new MimeType(null);
            var parts = ExtractParts(value);
            return new MimeType(parts.code, parts.attributes);
        }
    }
}
