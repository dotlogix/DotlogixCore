using System;

namespace DotLogix.Core.Nodes.Io {
    [Flags]
    public enum NodeIoOpCodes
    {
        None = 0,
        BeginMap = 1 << 0,
        EndMap = 1 << 1,
        BeginList = 1 << 2,
        EndList = 1 << 3,
        WriteValue = 1 << 4,
        SetName = 1 << 5,
        AutoComplete = 1 << 6,
        BeginAny = BeginMap | BeginList | WriteValue
    }
}