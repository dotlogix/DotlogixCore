// ==================================================
// Copyright 2016(C) , DotLogix
// File:  WindowToolBar.xaml.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Windows;
using System.Windows.Input;
#endregion

namespace DotLogix.UI.Controls {
    /// <summary>
    ///     Interaction logic for WindowToolBar.xaml
    /// </summary>
    public partial class WindowToolBar {
        public static readonly DependencyProperty ParentWindowProperty = DependencyProperty.Register(
                                                                                                     "ParentWindow",
                                                                                                     typeof(Window),
                                                                                                     typeof(
                                                                                                         WindowToolBar),
                                                                                                     new
                                                                                                     PropertyMetadata
                                                                                                     (default(Window),
                                                                                                      ParentWindow_Changed))
        ;

        public static readonly DependencyProperty AdditionalContentProperty = DependencyProperty.Register(
                                                                                                          "AdditionalContent",
                                                                                                          typeof(object
                                                                                                          ),
                                                                                                          typeof(
                                                                                                              WindowToolBar
                                                                                                          ),
                                                                                                          new
                                                                                                          PropertyMetadata
                                                                                                          (default(
                                                                                                               object)))
        ;

        public object AdditionalContent {
            get { return GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }

        public Window ParentWindow {
            get { return (Window)GetValue(ParentWindowProperty); }
            set { SetValue(ParentWindowProperty, value); }
        }

        public WindowToolBar() {
            InitializeComponent();
        }

        private static void ParentWindow_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            //var window = e.NewValue as Window;
            //if (window == null)
            //    return;

            //WindowChrome chrome=new WindowChrome();

            //WindowChrome.SetWindowChrome(window,chrome);
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) {
            var window = ParentWindow;
            if(window != null)
                window.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e) {
            var window = ParentWindow;
            if(window != null)
                window.WindowState = window.WindowState == WindowState.Maximized
                                         ? WindowState.Normal
                                         : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e) {
            ParentWindow?.Close();
        }

        private void WindowToolBar_OnMouseMove(object sender, MouseEventArgs e) {
            if(e.LeftButton != MouseButtonState.Pressed)
                return;
            var window = ParentWindow;
            if(window == null)
                return;
            if(window.WindowState == WindowState.Maximized)
                StateBackToNormal(window);
            window.DragMove();
        }

        private void StateBackToNormal(Window parentWindow) {
            var mp = Mouse.GetPosition(this);
            double[] mpFloat = {mp.X / RenderSize.Width, mp.Y / RenderSize.Height};
            parentWindow.WindowState = WindowState.Normal;
            mp = Mouse.GetPosition(this);
            var location =
            Point.Subtract(mp, new Vector(RenderSize.Width * mpFloat[0], RenderSize.Height * mpFloat[1]));
            BeginInit();
            parentWindow.Left = location.X;
            parentWindow.Top = location.Y;
            EndInit();
        }
    }
}
