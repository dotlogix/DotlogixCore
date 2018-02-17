// ==================================================
// Copyright 2018(C) , DotLogix
// File:  RadialGradientBrushKeyFrameCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Windows;
using System.Windows.Media;
#endregion

namespace DotLogix.UI.Animations {
    public class RadialGradientBrushKeyFrameCollection : FreezableCollection<KeyFrame<RadialGradientBrush>> {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new RadialGradientBrushKeyFrameCollection();
        }
        #endregion
    }
}
