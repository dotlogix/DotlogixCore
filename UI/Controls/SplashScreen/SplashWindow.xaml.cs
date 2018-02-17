// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SplashWindow.xaml.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
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
