// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Native.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
using System.Runtime.InteropServices;
using DotLogix.Core.Interop.Structs;
#endregion

namespace DotLogix.Core.Interop {
    public static class Native {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage([In] IntPtr hWnd, [In] uint msg,
                                                [In] IntPtr wParam, [In] IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow([In] IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow([In] IntPtr hWnd, [In] int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr
        FindWindowByCaption([In] IntPtr zeroOnly, [In] string lpWindowName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GlobalMemoryStatusEx([In] [Out] ref MemoryStateEx lpBuffer);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetSystemTimes([Out] out FileTime idleTime, [Out] out FileTime kernelTime,
                                                 [Out] out FileTime userTime);

        [return: MarshalAs(UnmanagedType.U4)]
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetLastError();
    }
}
