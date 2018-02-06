// ==================================================
// Copyright 2016(C) , DotLogix
// File:  InteropWindow.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.09.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Diagnostics;
using System.Linq;
#endregion

namespace DotLogix.Core.Interop {
    #region Using Directives
    #endregion

    public class InteropWindow {
        private readonly IntPtr _mainWindowHandle;

        private InteropWindow(IntPtr mainWindowHandle) {
            _mainWindowHandle = mainWindowHandle;
        }

        public void Close() {
            Interop.CloseWindow(_mainWindowHandle);
        }

        public void BringToFront() {
            Native.SetForegroundWindow(_mainWindowHandle);
        }

        public void Maximize() {
            Interop.MaximizeWindow(_mainWindowHandle);
        }

        public void Normal() {
            Interop.RestoreWindow(_mainWindowHandle);
        }

        public void Minimize() {
            Interop.MinimizeWindow(_mainWindowHandle);
        }

        public void ShowWindow() {
            Interop.ShowWindow(_mainWindowHandle);
        }

        public void HideWindow() {
            Interop.HideWindow(_mainWindowHandle);
        }

        public void SetWindowFlag(Interop.WindowFlags flag) {
            Interop.SetWindowFlag(_mainWindowHandle, flag);
        }

        public static InteropWindow ByTitle(string title) {
            var hwnd = Native.FindWindowByCaption(IntPtr.Zero, title);
            return hwnd == IntPtr.Zero ? null : ByHwnd(hwnd);
        }

        public static InteropWindow ByHwnd(IntPtr hwnd) {
            return new InteropWindow(hwnd);
        }

        public static InteropWindow ByProcessName(string processName) {
            return Process.GetProcessesByName(processName).Select(ByProcess).FirstOrDefault(proc => proc != null);
        }

        public static InteropWindow ByProcess(Process process) {
            try {
                if(process.MainWindowHandle != IntPtr.Zero)
                    return ByHwnd(process.MainWindowHandle);
            } catch {
                // ignored
            }

            return null;
        }
    }
}
