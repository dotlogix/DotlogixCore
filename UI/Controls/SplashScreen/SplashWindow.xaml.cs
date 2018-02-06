// ==================================================
// Copyright 2016(C) , DotLogix
// File:  SplashWindow.xaml.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

namespace DotLogix.UI.Controls.SplashScreen {
    /// <summary>
    ///     Interaction logic for SplashWindow.xaml
    /// </summary>
    public partial class SplashWindow {
        public SplashWindow() {
            InitializeComponent();
        }

        public void SetText(string text) {
            if(Dispatcher.CheckAccess() == false) {
                Dispatcher.Invoke(() => SetText(text));
                return;
            }
            LoadingTextBlock.Text = text;
        }
    }
}
