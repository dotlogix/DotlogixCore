// ==================================================
// Copyright 2016(C) , DotLogix
// File:  FrameworkElementExtension.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System.Windows;
#endregion

namespace DotLogix.UI.Extensions {
    public static class FrameworkElementExtension {
        public static TTag GetTagValue<TTag>(this FrameworkElement sender) {
            var tag = sender.Tag;
            if(tag is TTag)
                return (TTag)tag;
            return default(TTag);
        }

        public static TTag GetTagValue<TTag>(this object sender) {
            var fElement = sender as FrameworkElement;
            return fElement == null ? default(TTag) : fElement.GetTagValue<TTag>();
        }

        public static TTag GetDataContext<TTag>(this FrameworkElement sender) {
            var dataContext = sender.DataContext;
            if(dataContext is TTag)
                return (TTag)dataContext;
            return default(TTag);
        }

        public static TTag GetDataContext<TTag>(this object sender) {
            var fElement = sender as FrameworkElement;
            return fElement == null ? default(TTag) : fElement.GetDataContext<TTag>();
        }
    }
}
