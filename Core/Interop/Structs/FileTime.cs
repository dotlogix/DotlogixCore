// ==================================================
// Copyright 2016(C) , DotLogix
// File:  FileTime.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Runtime.InteropServices;
#endregion

namespace DotLogix.Core.Interop.Structs {
    [StructLayout(LayoutKind.Explicit)]
    public struct FileTime {
        /// <summary>
        ///     Specifies the high 32 bits of the FILETIME.
        /// </summary>
        [FieldOffset(32)]
        public uint HighDateTime;

        /// <summary>
        ///     Specifies the low 32 bits of the FILETIME.
        /// </summary>
        [FieldOffset(0)]
        public uint LowDateTime;

        public DateTime ToDateTime() {
            return DateTime.FromFileTime(AsLong());
        }

        public TimeSpan ToTimeSpan() {
            return TimeSpan.FromMilliseconds(AsLong() / 1000000D);
        }

        public long AsLong() {
            return LowDateTime | ((long)HighDateTime << 32);
        }
    }
}
