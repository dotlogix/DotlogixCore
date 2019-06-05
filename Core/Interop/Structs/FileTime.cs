// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FileTime.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Runtime.InteropServices;
#pragma warning disable 1591
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
