﻿// ==================================================
// Copyright 2016(C) , DotLogix
// File:  ExtendedWindow.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Windows;
using System.Windows.Input;
#endregion

namespace DotLogix.UI.Controls {
    public class ExtendedWindow : Window {
        public static readonly DependencyProperty AdditionalToolBarContentProperty = DependencyProperty.Register(
                                                                                                                 "AdditionalToolBarContent",
                                                                                                                 typeof(
                                                                                                                     object
                                                                                                                 ),
                                                                                                                 typeof(
                                                                                                                     ExtendedWindow
                                                                                                                 ),
                                                                                                                 new
                                                                                                                 PropertyMetadata
                                                                                                                 (default
                                                                                                                  (
                                                                                                                      object
                                                                                                                  )));

        public object AdditionalToolBarContent {
            get { return GetValue(AdditionalToolBarContentProperty); }
            set { SetValue(AdditionalToolBarContentProperty, value); }
        }

        public ExtendedWindow() {
            //var windowStyle = Application.Current.FindResource("WindowStyle") as Style;
            //Style = windowStyle;
            DefaultStyleKey = typeof(ExtendedWindow);
            SetResourceReference(StyleProperty, typeof(ExtendedWindow));
            KeyDown += Window_OnKeyDown;
        }


        private void Window_OnKeyDown(object sender, KeyEventArgs e) {
            switch(e.Key) {
                case Key.F9:
                    SwitchToMinimized();
                    break;
                case Key.F10:
                    SwitchToNormal();
                    break;
                case Key.F11:
                    SwitchToMaximized();
                    break;
                case Key.F12:
                    SwitchToWorkingArea();
                    break;
                case Key.System:
                    if(e.SystemKey == Key.F10)
                        SwitchToNormal();
                    break;
            }
        }


        public void SwitchToNormal() {
            Width = 800;
            Height = 600;
            WindowState = WindowState.Normal;
        }

        public void SwitchToMaximized() {
            if(WindowState == WindowState.Maximized)
                SwitchToNormal();
            else
                WindowState = WindowState.Maximized;
        }

        public void SwitchToMinimized() {
            WindowState = WindowState.Minimized;
        }

        public void SwitchToWorkingArea() {
            var rect = SystemParameters.WorkArea;
            Left = rect.Left;
            Top = rect.Top;
            Width = rect.Width;
            Height = rect.Height;
            WindowState = WindowState.Normal;
        }
    }
}
