// ==================================================
// Copyright 2018(C) , DotLogix
// File:  NavPage.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
#endregion

namespace DotLogix.UI.Navigation {
    [ContentProperty("Content")]
    public class NavPage : Control {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
                                                                                                "Content",
                                                                                                typeof(UIElement),
                                                                                                typeof(NavPage),
                                                                                                new
                                                                                                PropertyMetadata(default
                                                                                                                 (UIElement
                                                                                                                 )));

        public NavService NavService { get; internal set; }

        public UIElement Content {
            get => (UIElement)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public NavPage() {
            DefaultStyleKey = typeof(NavPage);
            SetResourceReference(StyleProperty, typeof(NavPage));
        }
    }
}
