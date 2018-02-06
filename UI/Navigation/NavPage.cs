// ==================================================
// Copyright 2016(C) , DotLogix
// File:  NavPage.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
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
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public NavPage() {
            DefaultStyleKey = typeof(NavPage);
            SetResourceReference(StyleProperty, typeof(NavPage));
        }
    }
}
