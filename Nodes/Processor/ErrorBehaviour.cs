using System;

namespace DotLogix.Core.Nodes.Processor {
    [Flags]
    public enum ErrorBehaviour {
        Unhandled,
        SkipCharacter,
        SkipToEnd,
        Accept
    }
}