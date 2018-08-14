// ==================================================
// Copyright 2018(C) , DotLogix
// File:  Interop.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System;
#endregion

namespace DotLogix.Core.Interop {
    #region Using Directives
    #endregion

    public class Interop {
        public enum WindowFlags {
            SwForceminimize = 11,
            SwHide = 0,
            SwMaximize = 3,
            SwMinimize = 6,
            SwRestore = 9,
            SwShow = 5,
            SwShowdefault = 10,
            SwShowmaximized = 3,
            SwShowminimized = 2,
            SwShowminnoactive = 7,
            SwShowna = 8,
            SwShownoactivate = 4,
            SwShownormal = 1
        }

        internal static void CloseWindow(IntPtr mainWindowHandle) {
            Native.SendMessage(mainWindowHandle, 0x0010, IntPtr.Zero, IntPtr.Zero);
        }

        internal static void MaximizeWindow(IntPtr mainWindowHandle) {
            Native.ShowWindow(mainWindowHandle, (int)WindowFlags.SwMaximize);
        }

        internal static void MinimizeWindow(IntPtr mainWindowHandle) {
            Native.ShowWindow(mainWindowHandle, (int)WindowFlags.SwMinimize);
        }

        internal static void RestoreWindow(IntPtr mainWindowHandle) {
            Native.ShowWindow(mainWindowHandle, (int)WindowFlags.SwRestore);
        }

        internal static void ShowWindow(IntPtr mainWindowHandle) {
            Native.ShowWindow(mainWindowHandle, (int)WindowFlags.SwShow);
        }

        internal static void HideWindow(IntPtr mainWindowHandle) {
            Native.ShowWindow(mainWindowHandle, (int)WindowFlags.SwHide);
        }

        internal static void SetWindowFlag(IntPtr mainWindowHandle, WindowFlags flag) {
            Native.ShowWindow(mainWindowHandle, (int)flag);
        }
    }
}
