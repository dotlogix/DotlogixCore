// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Settings.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Collections.Generic;
using DotLogix.Core.Diagnostics;
using DotLogix.Core.Rest.Http.Parameters;
using DotLogix.Core.Utils;
#endregion

namespace DotLogix.Core.Rest {
    public class WebServerSettings {
        public bool EnableLogging {
            get => LogSource.Enabled;
            set => LogSource.Enabled = value;
        }
        public ILogSource LogSource { get; set; } = Log.CreateSource("WebServer");
        public IParameterParser ParameterParser { get; set; } = PrimitiveParameterParser.Default;
        public ISet<string> UrlPrefixes { get; } = new HashSet<string>();

        public bool EnableConcurrentRequests { get; set; }
        public int RequestLimit { get; set; }
        public int WebSocketLimit { get; set; }


        public bool EnableBodyBuffering { get; set; }
        public string TempDirectory { get; set; }
        public long MaxBodyLength { get; set; } = 10_485_760L;
        public long MaxMemoryBufferSize { get; set; } = 10_485_760L;
        public long MaxFileBufferSize { get; set; } = 104_857_600L;
    }
}
