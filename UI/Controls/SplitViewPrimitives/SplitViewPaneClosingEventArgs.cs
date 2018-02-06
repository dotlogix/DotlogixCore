// ==================================================
// Copyright 2016(C) , DotLogix
// File:  SplitViewPaneClosingEventArgs.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
#endregion

namespace DotLogix.UI.Controls.SplitViewPrimitives {
    /// <summary>
    ///     Provides event data for the SplitView.PaneClosing event.
    /// </summary>
    public sealed class SplitViewPaneClosingEventArgs : EventArgs {
        /// <summary>
        ///     Gets or sets a value that indicates whether the pane closing action should be
        ///     canceled.
        /// </summary>
        /// <value>true to cancel the pane closing action; otherwise, false.</value>
        public bool Cancel { get; set; }

        internal SplitViewPaneClosingEventArgs() { }
    }
}
