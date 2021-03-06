// ==================================================
// Copyright 2018(C) , DotLogix
// File:  SplitViewTemplateSettings.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Windows;
#endregion

namespace DotLogix.UI.Controls.SplitViewPrimitives {
    /// <summary>
    ///     Provides calculated values that can be referenced as TemplatedParent sources when defining templates for a
    ///     <see cref="SplitView" />.
    ///     Not intended for general use.
    /// </summary>
    public sealed class SplitViewTemplateSettings : DependencyObject {
        internal SplitView Owner { get; }

        internal SplitViewTemplateSettings(SplitView owner) {
            Owner = owner;
            Update();
        }

        internal void Update() {
            CompactPaneGridLength = new GridLength(Owner.CompactPaneLength, GridUnitType.Pixel);
            NegativeOpenPaneLength = -Owner.OpenPaneLength;
            NegativeOpenPaneLengthMinusCompactLength = -(Owner.OpenPaneLength - Owner.CompactPaneLength);
            OpenPaneGridLength = new GridLength(Owner.OpenPaneLength, GridUnitType.Pixel);
            OpenPaneLength = Owner.OpenPaneLength;
            OpenPaneLengthMinusCompactLength = Owner.OpenPaneLength - Owner.CompactPaneLength;
        }

        #region CompactPaneGridLength
        /// <summary>
        ///     Gets the <see cref="SplitView.CompactPaneLength" /> value as a GridLength.
        /// </summary>
        public GridLength CompactPaneGridLength {
            get => (GridLength)GetValue(CompactPaneGridLengthProperty);
            private set => SetValue(CompactPaneGridLengthProperty, value);
        }

        internal static readonly DependencyProperty CompactPaneGridLengthProperty =
        DependencyProperty.Register("CompactPaneGridLength", typeof(GridLength), typeof(SplitViewTemplateSettings),
                                    new PropertyMetadata(null));
        #endregion

        #region NegativeOpenPaneLength
        /// <summary>
        ///     Gets the negative of the <see cref="SplitView.OpenPaneLength" /> value.
        /// </summary>
        public double NegativeOpenPaneLength {
            get => (double)GetValue(NegativeOpenPaneLengthProperty);
            private set => SetValue(NegativeOpenPaneLengthProperty, value);
        }

        internal static readonly DependencyProperty NegativeOpenPaneLengthProperty =
        DependencyProperty.Register("NegativeOpenPaneLength", typeof(double), typeof(SplitViewTemplateSettings),
                                    new PropertyMetadata(0d));
        #endregion

        #region NegativeOpenPaneLengthMinusCompactLength
        /// <summary>
        ///     Gets the negative of the value calculated by subtracting the <see cref="SplitView.CompactPaneLength" /> value from
        ///     the <see cref="SplitView.OpenPaneLength" /> value.
        /// </summary>
        public double NegativeOpenPaneLengthMinusCompactLength {
            get => (double)GetValue(NegativeOpenPaneLengthMinusCompactLengthProperty);
            set => SetValue(NegativeOpenPaneLengthMinusCompactLengthProperty, value);
        }

        internal static readonly DependencyProperty NegativeOpenPaneLengthMinusCompactLengthProperty =
        DependencyProperty.Register("NegativeOpenPaneLengthMinusCompactLength", typeof(double),
                                    typeof(SplitViewTemplateSettings), new PropertyMetadata(0d));
        #endregion

        #region OpenPaneGridLength
        /// <summary>
        ///     Gets the <see cref="SplitView.OpenPaneLength" /> value as a GridLength.
        /// </summary>
        public GridLength OpenPaneGridLength {
            get => (GridLength)GetValue(OpenPaneGridLengthProperty);
            private set => SetValue(OpenPaneGridLengthProperty, value);
        }

        internal static readonly DependencyProperty OpenPaneGridLengthProperty =
        DependencyProperty.Register("OpenPaneGridLength", typeof(GridLength), typeof(SplitViewTemplateSettings),
                                    new PropertyMetadata(null));
        #endregion

        #region OpenPaneLength
        /// <summary>
        ///     Gets the <see cref="SplitView.OpenPaneLength" /> value.
        /// </summary>
        public double OpenPaneLength {
            get => (double)GetValue(OpenPaneLengthProperty);
            private set => SetValue(OpenPaneLengthProperty, value);
        }

        internal static readonly DependencyProperty OpenPaneLengthProperty =
        DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(SplitViewTemplateSettings),
                                    new PropertyMetadata(0d));
        #endregion

        #region OpenPaneLengthMinusCompactLength
        /// <summary>
        ///     Gets a value calculated by subtracting the <see cref="SplitView.CompactPaneLength" /> value from the
        ///     <see cref="SplitView.OpenPaneLength" /> value.
        /// </summary>
        public double OpenPaneLengthMinusCompactLength {
            get => (double)GetValue(OpenPaneLengthMinusCompactLengthProperty);
            private set => SetValue(OpenPaneLengthMinusCompactLengthProperty, value);
        }

        internal static readonly DependencyProperty OpenPaneLengthMinusCompactLengthProperty =
        DependencyProperty.Register("OpenPaneLengthMinusCompactLength", typeof(double),
                                    typeof(SplitViewTemplateSettings), new PropertyMetadata(0d));
        #endregion
    }
}
