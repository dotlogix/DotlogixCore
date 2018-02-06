using System;

namespace DotLogix.Core.Nodes.Io {
    [Flags]
    public enum NodeIoState
    {
        None,
        InsideMap,
        InsideList
    }
}