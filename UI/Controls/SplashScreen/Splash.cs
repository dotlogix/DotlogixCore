// ==================================================
// Copyright 2016(C) , DotLogix
// File:  Splash.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Threading;
#endregion

namespace DotLogix.UI.Controls.SplashScreen {
    public static class Splash {
        private static SplashWindow _window;
        private static Thread _splashThread;
        private static readonly ManualResetEvent WindowWait = new ManualResetEvent(false);

        public static void SetLoadingText(string text) {
            if(_splashThread == null)
                return;
            WindowWait.WaitOne();
            _window.SetText(text);
        }

        public static void Show() {
            _splashThread = new Thread(SplashMain) {IsBackground = true};
            _splashThread.SetApartmentState(ApartmentState.STA);
            _splashThread.Start();
        }

        private static void SplashMain() {
            _window = new SplashWindow();
            WindowWait.Set();
            _window.ShowDialog();
        }

        public static void Close() {
            if(_splashThread == null)
                return;
            WindowWait.WaitOne();
            _window.Dispatcher.Invoke(() => _window.Close());
            _splashThread.Join();
            WindowWait.Reset();
            _splashThread = null;
        }
    }
}
