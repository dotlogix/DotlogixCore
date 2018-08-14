// ==================================================
// Copyright 2018(C) , DotLogix
// File:  GridLengthKeyFrameCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Windows;
#endregion

namespace DotLogix.UI.Animations {
    public class GridLengthKeyFrameCollection : FreezableCollection<KeyFrame<GridLength>> {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new GridLengthKeyFrameCollection();
        }
        #endregion
    }
}
