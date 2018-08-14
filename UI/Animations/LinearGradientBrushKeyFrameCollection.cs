// ==================================================
// Copyright 2018(C) , DotLogix
// File:  LinearGradientBrushKeyFrameCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Windows;
using System.Windows.Media;
#endregion

namespace DotLogix.UI.Animations {
    public class LinearGradientBrushKeyFrameCollection : FreezableCollection<KeyFrame<LinearGradientBrush>> {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new LinearGradientBrushKeyFrameCollection();
        }
        #endregion
    }
}
