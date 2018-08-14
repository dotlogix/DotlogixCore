// ==================================================
// Copyright 2018(C) , DotLogix
// File:  FrameworkElementExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Windows;
#endregion

namespace DotLogix.UI.Extensions {
    public static class FrameworkElementExtension {
        public static TTag GetTagValue<TTag>(this FrameworkElement sender) {
            var tag = sender.Tag;
            return tag is TTag t ? t : default;
        }

        public static TTag GetTagValue<TTag>(this object sender) {
            return !(sender is FrameworkElement fElement) ? default : fElement.GetTagValue<TTag>();
        }

        public static TTag GetDataContext<TTag>(this FrameworkElement sender) {
            var dataContext = sender.DataContext;
            return dataContext is TTag t ? t : default;
        }

        public static TTag GetDataContext<TTag>(this object sender) {
            return !(sender is FrameworkElement fElement) ? default : fElement.GetDataContext<TTag>();
        }
    }
}
