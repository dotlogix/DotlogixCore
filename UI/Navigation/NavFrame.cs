// ==================================================
// Copyright 2016(C) , DotLogix
// File:  NavFrame.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
#endregion

namespace DotLogix.UI.Navigation {
    [ContentProperty("Page")]
    public class NavFrame : Control {
        public static readonly DependencyProperty PageProperty = DependencyProperty.Register(
                                                                                             "Page", typeof(NavPage),
                                                                                             typeof(NavFrame),
                                                                                             new
                                                                                             PropertyMetadata(default(
                                                                                                                  NavPage
                                                                                                              )));

        private NavService _navService;

        public NavService NavService {
            get { return _navService; }
            set {
                if(!Equals(_navService.NavFrame, this))
                    throw new InvalidOperationException("The navframe of the navigation service is not valid");
                _navService = value;
                Page = _navService.CurrentPage;
            }
        }

        public NavPage Page {
            get { return (NavPage)GetValue(PageProperty); }
            internal set { SetValue(PageProperty, value); }
        }

        public NavFrame() {
            DefaultStyleKey = typeof(NavFrame);
            _navService = new NavService(this);
        }
    }
}
