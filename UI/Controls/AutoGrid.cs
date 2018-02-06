// ==================================================
// Copyright 2016(C) , DotLogix
// File:  AutoGrid.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace DotLogix.UI.Controls {
    public class AutoGrid : Panel {
        public static readonly DependencyProperty MinItemWidthProperty = DependencyProperty.Register(
                                                                                                     "MinItemWidth",
                                                                                                     typeof(double),
                                                                                                     typeof(AutoGrid),
                                                                                                     new
                                                                                                     PropertyMetadata
                                                                                                     (1d));

        public static readonly DependencyProperty MaxItemWidthProperty = DependencyProperty.Register(
                                                                                                     "MaxItemWidth",
                                                                                                     typeof(double),
                                                                                                     typeof(AutoGrid),
                                                                                                     new
                                                                                                     PropertyMetadata
                                                                                                     (double.MaxValue));

        public static readonly DependencyProperty MinItemHeightProperty = DependencyProperty.Register(
                                                                                                      "MinItemHeight",
                                                                                                      typeof(double),
                                                                                                      typeof(AutoGrid),
                                                                                                      new
                                                                                                      PropertyMetadata
                                                                                                      (1d));

        public static readonly DependencyProperty MaxItemHeightProperty = DependencyProperty.Register(
                                                                                                      "MaxItemHeight",
                                                                                                      typeof(double),
                                                                                                      typeof(AutoGrid),
                                                                                                      new
                                                                                                      PropertyMetadata
                                                                                                      (double.MaxValue))
        ;

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
                                                                                                    "Orientation",
                                                                                                    typeof(Orientation),
                                                                                                    typeof(AutoGrid),
                                                                                                    new PropertyMetadata
                                                                                                    (Orientation.
                                                                                                     Horizontal));

        public static readonly DependencyProperty LimitToChildCountProperty = DependencyProperty.Register(
                                                                                                          "LimitToChildCount",
                                                                                                          typeof(bool),
                                                                                                          typeof(
                                                                                                              AutoGrid),
                                                                                                          new
                                                                                                          PropertyMetadata
                                                                                                          (default(bool
                                                                                                           )));

        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(
                                                                                                    "ItemPadding",
                                                                                                    typeof(double),
                                                                                                    typeof(AutoGrid),
                                                                                                    new PropertyMetadata
                                                                                                    (default(double)));

        public double ItemPadding {
            get { return (double)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }

        public double MinItemWidth {
            get { return (double)GetValue(MinItemWidthProperty); }
            set { SetValue(MinItemWidthProperty, value); }
        }

        public double MaxItemWidth {
            get { return (double)GetValue(MaxItemWidthProperty); }
            set { SetValue(MaxItemWidthProperty, value); }
        }

        public double MinItemHeight {
            get { return (double)GetValue(MinItemHeightProperty); }
            set { SetValue(MinItemHeightProperty, value); }
        }

        public double MaxItemHeight {
            get { return (double)GetValue(MaxItemHeightProperty); }
            set { SetValue(MaxItemHeightProperty, value); }
        }

        public bool LimitToChildCount {
            get { return (bool)GetValue(LimitToChildCountProperty); }
            set { SetValue(LimitToChildCountProperty, value); }
        }

        public Orientation Orientation {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize) {
            GetSize(availableSize, out Size itemSize, out Size realSize);
            foreach(UIElement child in Children) {
                child.Measure(itemSize);
            }
            return realSize;
        }

        private void GetSize(Size availableSize, out Size itemSize, out Size realSize) {
            var childCount = Children.Count;
            if(childCount == 0) {
                itemSize = new Size(0, 0);
                realSize = new Size(0, 0);
                return;
            }

            var padding = ItemPadding;
            var minItemHeight = MinItemHeight;
            var minItemWidth = MinItemWidth;
            var maxItemWidth = MaxItemWidth;
            var maxItemHeight = MaxItemHeight;
            var paddedMinItemWidth = minItemWidth + padding;
            var paddedMinItemHeight = minItemHeight + padding;
            realSize = availableSize;
            if(Orientation == Orientation.Horizontal) {
                if(double.IsNaN(availableSize.Width) || double.IsInfinity(availableSize.Width)) {
                    realSize.Width = (paddedMinItemWidth * childCount) - padding;
                    realSize.Height = minItemHeight;
                    itemSize = new Size(minItemWidth, Math.Min(maxItemHeight, realSize.Height));
                } else {
                    var childsPerRow = (int)((realSize.Width + padding) / paddedMinItemWidth);
                    if(LimitToChildCount && (childsPerRow > childCount))
                        childsPerRow = childCount;
                    var requiredRows = Math.Ceiling(childCount / (double)childsPerRow);
                    realSize.Height = (paddedMinItemHeight * requiredRows) - padding;
                    itemSize =
                    new Size(
                             Math.Min(maxItemWidth,
                                      (availableSize.Width - (padding * (childsPerRow - 1))) / childsPerRow),
                             MinItemHeight);
                }
            } else {
                if(double.IsNaN(availableSize.Height) || double.IsInfinity(availableSize.Height)) {
                    realSize.Width = minItemWidth;
                    realSize.Height = (paddedMinItemHeight * childCount) - padding;
                    itemSize = new Size(Math.Min(maxItemWidth, availableSize.Width), minItemHeight);
                } else {
                    var childsPerColumn = (int)((realSize.Height + padding) / paddedMinItemHeight);
                    if(LimitToChildCount && (childsPerColumn > childCount))
                        childsPerColumn = childCount;
                    var requiredColumns = Math.Ceiling(childCount / (double)childsPerColumn);
                    realSize.Width = (paddedMinItemWidth * requiredColumns) - padding;
                    itemSize = new Size(MinItemWidth,
                                        Math.Min(MaxItemHeight,
                                                 (availableSize.Height - (padding * (childsPerColumn - 1))) /
                                                 childsPerColumn));
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize) {
            GetSize(finalSize, out Size itemSize, out Size realSize);
            var padding = ItemPadding;
            var paddedItemWidth = itemSize.Width + padding;
            var paddedItemHeight = itemSize.Height + padding;
            if(Orientation == Orientation.Horizontal) {
                var itemsPerRow = (int)Math.Round(realSize.Width / itemSize.Width);
                for(var i = 0; i < Children.Count; i++) {
                    var x = i % itemsPerRow;
                    var y = i / itemsPerRow;
                    var pos = new Point(x * paddedItemWidth, y * paddedItemHeight);
                    Children[i].Arrange(new Rect(pos, itemSize));
                }
            } else {
                var itemsPerColumn = (int)Math.Round(realSize.Height / itemSize.Height);
                for(var i = 0; i < Children.Count; i++) {
                    var x = i / itemsPerColumn;
                    var y = i % itemsPerColumn;
                    var pos = new Point(x * paddedItemWidth, y * paddedItemHeight);
                    Children[i].Arrange(new Rect(pos, itemSize));
                }
            }
            return realSize;
        }
    }
}
