#region
using System;
#endregion

namespace DotLogix.Core.Nodes.Formats.Json {
    [Flags]
    public enum ErrorBehaviour {
        Unhandled = -1,
        Ok = 0,
        SkipCharacter = 1,
        SkipToEnd = 2,
    }
}
