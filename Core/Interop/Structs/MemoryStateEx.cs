// ==================================================
// Copyright 2018(C) , DotLogix
// File:  MemoryStateEx.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Runtime.InteropServices;
#pragma warning disable 1591
#endregion

namespace DotLogix.Core.Interop.Structs {
    /// <summary>
    ///     If your using this structure you have to set the Value of Length to 64
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct MemoryStateEx {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.U4)]
        public uint Length;

        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.U4)]
        public uint MemoryLoad;

        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.U8)]
        public ulong TotalPhys;

        [FieldOffset(16)]
        [MarshalAs(UnmanagedType.U8)]
        public ulong AvailPhys;

        [FieldOffset(24)]
        [MarshalAs(UnmanagedType.U8)]
        public ulong TotalPageFile;

        [FieldOffset(32)]
        [MarshalAs(UnmanagedType.U8)]
        public ulong AvailPageFile;

        [FieldOffset(40)]
        [MarshalAs(UnmanagedType.U8)]
        public ulong TotalVirtual;

        [FieldOffset(48)]
        [MarshalAs(UnmanagedType.U8)]
        public ulong AvailVirtual;

        [FieldOffset(56)]
        [MarshalAs(UnmanagedType.U8)]
        public ulong AvailExtendedVirtual;
    }
}
