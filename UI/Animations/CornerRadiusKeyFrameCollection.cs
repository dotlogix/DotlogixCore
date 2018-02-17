// ==================================================
// Copyright 2018(C) , DotLogix
// File:  CornerRadiusKeyFrameCollection.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System.Windows;
#endregion

namespace DotLogix.UI.Animations {
    public class CornerRadiusKeyFrameCollection : FreezableCollection<KeyFrame<CornerRadius>> {
        #region Freezable
        protected override Freezable CreateInstanceCore() {
            return new CornerRadiusKeyFrameCollection();
        }
        #endregion
    }
}
