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
    public class WebServerSettings : PrefixedSettings {/// <inheritdoc />
        public WebServerSettings() : this(null) { }
        public WebServerSettings(ISettings settings, string prefix = null) : base(settings, prefix) {
            LogSource ??= Log.CreateSource("WebServer");
            UrlPrefixes ??= new List<string>();
        }


        public ILogSource LogSource {
            get => GetWithMemberName<ILogSource>();
            set => SetWithMemberName(value);
        }

        public IParameterParser ParameterParser {
            get => GetWithMemberName(PrimitiveParameterParser.Default);
            set => SetWithMemberName(value);
        }

        public ICollection<string> UrlPrefixes {
            get => GetWithMemberName<ICollection<string>>();
            set => SetWithMemberName(value);
        }

        public bool EnableConcurrentRequests {
            get => GetWithMemberName(true);
            set => SetWithMemberName(value);
        }

        public int RequestLimit {
            get => GetWithMemberName(64);
            set => SetWithMemberName(value);
        }

        public int WebSocketLimit {
            get => GetWithMemberName(64);
            set => SetWithMemberName(value);
        }


        public bool EnableBodyBuffering {
            get => GetWithMemberName(true);
            set => SetWithMemberName(value);
        }

        public string TempDirectory {
            get => GetWithMemberName<string>();
            set => SetWithMemberName(value);
        }

        public long MaxBodyLength {
            get => GetWithMemberName(104_857_600L); // 100MB
            set => SetWithMemberName(value);
        }

        public long MaxMemoryBufferSize {
            get => GetWithMemberName(104_857_600L); // 10MB
            set => SetWithMemberName(value);
        }

        public long MaxFileBufferSize {
            get => GetWithMemberName(104_857_600L); // 100MB
            set => SetWithMemberName(value);
        }
    }
}
