// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CheckButton.xaml.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  24.03.2018
// ==================================================

#region
using System;
using System.Windows;
using System.Windows.Media;
using DotLogix.UI.Extensions;
#endregion

namespace DotLogix.UI.Controls {
    /// <summary>
    ///     Interaktionslogik für CheckButton.xaml
    /// </summary>
    public partial class CheckButton {
        public static readonly DependencyProperty CheckAreaThicknessProperty = DependencyProperty.Register(
                                                                                                           "CheckAreaThickness", typeof(double), typeof(CheckButton), new PropertyMetadata(16D));

        public static RoutedEvent CheckedEvent = EventManager.RegisterRoutedEvent("Checked", RoutingStrategy.Bubble,
                                                                                  typeof(RoutedEventHandler),
                                                                                  typeof(CheckButton));

        public static RoutedEvent UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked", RoutingStrategy.Bubble,
                                                                                    typeof(RoutedEventHandler),
                                                                                    typeof(CheckButton));

        public static RoutedEvent CheckStateChangedEvent = EventManager.RegisterRoutedEvent("CheckStateChanged", RoutingStrategy.Bubble,
                                                                                    typeof(RoutedEventHandler),
                                                                                    typeof(CheckButton));

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
                                                                                                  "IsChecked",
                                                                                                  typeof(bool),
                                                                                                  typeof(CheckButton),
                                                                                                  new PropertyMetadata(
                                                                                                                       default
                                                                                                                       (
                                                                                                                           bool
                                                                                                                       )))
        ;

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                                                                                             "Text", typeof(string),
                                                                                             typeof(CheckButton),
                                                                                             new PropertyMetadata(
                                                                                                                  default
                                                                                                                  (
                                                                                                                      string
                                                                                                                  )));

        public static readonly DependencyProperty CheckedBrushProperty = DependencyProperty.Register(
                                                                                                     "CheckedBrush",
                                                                                                     typeof(Brush),
                                                                                                     typeof(CheckButton
                                                                                                     ),
                                                                                                     new
                                                                                                     PropertyMetadata
                                                                                                     (Brushes.
                                                                                                      DarkGreen));

        public static readonly DependencyProperty UncheckedBrushProperty = DependencyProperty.Register(
                                                                                                       "UncheckedBrush",
                                                                                                       typeof(Brush),
                                                                                                       typeof(
                                                                                                           CheckButton),
                                                                                                       new
                                                                                                       PropertyMetadata
                                                                                                       (Brushes.
                                                                                                        DarkGray));

        public double CheckAreaThickness {
            get => (double)GetValue(CheckAreaThicknessProperty);
            set => SetValue(CheckAreaThicknessProperty, value);
        }

        public bool IsChecked {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public string Text {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Brush CheckedBrush {
            get => (Brush)GetValue(CheckedBrushProperty);
            set => SetValue(CheckedBrushProperty, value);
        }

        public Brush UncheckedBrush {
            get => (Brush)GetValue(UncheckedBrushProperty);
            set => SetValue(UncheckedBrushProperty, value);
        }

        public CheckButton() {
            InitializeComponent();
            IsCheckedProperty.AddChangedHandler(this, OnTrigger);
        }

        public event RoutedEventHandler Unchecked {
            add => AddHandler(UncheckedEvent, value);
            remove => RemoveHandler(UncheckedEvent, value);
        }

        public event RoutedEventHandler Checked {
            add => AddHandler(CheckedEvent, value);
            remove => RemoveHandler(CheckedEvent, value);
        }

        public event RoutedEventHandler CheckStateChanged {
            add => AddHandler(CheckStateChangedEvent, value);
            remove => RemoveHandler(CheckStateChangedEvent, value);
        }

        private void OnTrigger(object sender, EventArgs e) {
            RaiseEvent(IsChecked ? new RoutedEventArgs(CheckedEvent) : new RoutedEventArgs(UncheckedEvent));
            RaiseEvent(new RoutedEventArgs(CheckStateChangedEvent));
        }

        private void CheckButton_OnClick(object sender, RoutedEventArgs e) {
            IsChecked = !IsChecked;
        }
    }
}
