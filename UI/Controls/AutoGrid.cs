// ==================================================
// Copyright 2018(C) , DotLogix
// File:  AutoGrid.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  06.02.2018
// LastEdited:  17.02.2018
// ==================================================

#region
using System;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace DotLogix.UI.Controls {
    public class AutoGrid : Panel {
        public static readonly DependencyProperty ItemGapXProperty = DependencyProperty.Register(
                                                        "ItemGapX", typeof(double), typeof(AutoGrid), new PropertyMetadata(default(double)));

        public double ItemGapX {
            get { return (double)GetValue(ItemGapXProperty); }
            set { SetValue(ItemGapXProperty, value); }
        }

        public static readonly DependencyProperty ItemGapYProperty = DependencyProperty.Register(
                                                        "ItemGapY", typeof(double), typeof(AutoGrid), new PropertyMetadata(default(double)));

        public double ItemGapY {
            get { return (double)GetValue(ItemGapYProperty); }
            set { SetValue(ItemGapYProperty, value); }
        }

        public static readonly DependencyProperty TargetWidthProperty = DependencyProperty.Register(
                                                        "TargetWidth", typeof(double), typeof(AutoGrid), new PropertyMetadata(default(double)));

        public double TargetWidth {
            get { return (double)GetValue(TargetWidthProperty); }
            set { SetValue(TargetWidthProperty, value); }
        }

        public static readonly DependencyProperty TargetHeightProperty = DependencyProperty.Register(
                                                        "TargetHeight", typeof(double), typeof(AutoGrid), new PropertyMetadata(default(double)));

        public double TargetHeight {
            get { return (double)GetValue(TargetHeightProperty); }
            set { SetValue(TargetHeightProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
                                                        "Orientation", typeof(Orientation), typeof(AutoGrid), new PropertyMetadata(default(Orientation)));

        public Orientation Orientation {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize) {
            GetSize(availableSize, out var itemSize, out var realSize);
            foreach (UIElement child in Children)
                child.Measure(itemSize);
            return realSize;
        }

        private void GetSize(Size availableSize, out Size itemSize, out Size realSize)
        {
            var childCount = Children.Count;
            if (childCount == 0)
            {
                itemSize = new Size(0, 0);
                realSize = new Size(0, 0);
                return;
            }

            var paddingX = ItemGapX;
            var paddingY = ItemGapY;
            var targetItemHeight = TargetHeight;
            var targetItemWidth = TargetWidth;
            var paddedTargetWidth = targetItemWidth + paddingX;
            var paddedTargetHeight = targetItemHeight + paddingY;

            realSize = availableSize;
            if (Orientation == Orientation.Horizontal)
            {
                if (double.IsNaN(availableSize.Width) || double.IsInfinity(availableSize.Width))
                {
                    realSize.Width = (paddedTargetWidth * childCount) - paddingX;
                    realSize.Height = targetItemHeight;
                    itemSize = new Size(targetItemWidth, Math.Min(targetItemHeight, realSize.Height));
                }
                else
                {
                    var childsPerRow = Math.Max((int)((realSize.Width + paddingX) / paddedTargetWidth), 1);
                    var requiredRows = Math.Ceiling(childCount / (double)childsPerRow);
                    realSize.Height = (paddedTargetHeight * requiredRows) - paddingY;
                    itemSize = new Size((availableSize.Width - (paddingX * (childsPerRow - 1))) / childsPerRow, targetItemHeight);
                }
            }
            else
            {
                if (double.IsNaN(availableSize.Height) || double.IsInfinity(availableSize.Height))
                {
                    realSize.Width = targetItemWidth;
                    realSize.Height = (paddedTargetHeight * childCount) - paddingY;
                    itemSize = new Size(Math.Min(targetItemWidth, availableSize.Width), targetItemHeight);
                }
                else
                {
                    var childsPerColumn = Math.Max((int)((realSize.Height + paddingY) / paddedTargetHeight), 1);
                    var requiredColumns = Math.Ceiling(childCount / (double)childsPerColumn);
                    realSize.Width = (paddedTargetWidth * requiredColumns) - paddingX;
                    itemSize = new Size(targetItemWidth, (availableSize.Height - (paddingY * (childsPerColumn - 1))) / childsPerColumn);
                }
            }
        }



        protected override Size ArrangeOverride(Size finalSize) {
            GetSize(finalSize, out var itemSize, out var realSize);
            var paddingX = ItemGapX;
            var paddingY = ItemGapY;
            var targetItemHeight = itemSize.Height;
            var targetItemWidth = itemSize.Width;
            var paddedTargetWidth = targetItemWidth + paddingX;
            var paddedTargetHeight = targetItemHeight + paddingY;
            if (Orientation == Orientation.Horizontal)
            {
                var itemsPerRow = (int)Math.Round(realSize.Width / itemSize.Width);
                for (var i = 0; i < Children.Count; i++)
                {
                    var x = i % itemsPerRow;
                    var y = i / itemsPerRow;
                    var pos = new Point(x * paddedTargetWidth, y * paddedTargetHeight);
                    Children[i].Arrange(new Rect(pos, itemSize));
                }
            }
            else
            {
                var itemsPerColumn = (int)Math.Round(realSize.Height / itemSize.Height);
                for (var i = 0; i < Children.Count; i++)
                {
                    var x = i / itemsPerColumn;
                    var y = i % itemsPerColumn;
                    var pos = new Point(x * paddedTargetWidth, y * paddedTargetHeight);
                    Children[i].Arrange(new Rect(pos, itemSize));
                }
            }
            return realSize;
        }
    }
}
