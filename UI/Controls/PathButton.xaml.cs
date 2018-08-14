// ==================================================
// Copyright 2018(C) , DotLogix
// File:  PathButton.xaml.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  17.02.2018
// LastEdited:  01.08.2018
// ==================================================

#region
using System.Windows;
using System.Windows.Media;
#endregion

namespace DotLogix.UI.Controls {
    /// <summary>
    ///     Interaction logic for PathButton.xaml
    /// </summary>
    public partial class PathButton {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
                                                                                             "Data", typeof(Geometry),
                                                                                             typeof(PathButton),
                                                                                             new PropertyMetadata(
                                                                                                                  default
                                                                                                                  (
                                                                                                                      Geometry
                                                                                                                  )));

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
                                                                                             "Fill", typeof(Brush),
                                                                                             typeof(PathButton),
                                                                                             new PropertyMetadata(
                                                                                                                  default
                                                                                                                  (Brush
                                                                                                                  )));

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
                                                                                               "Stroke", typeof(Brush),
                                                                                               typeof(PathButton),
                                                                                               new PropertyMetadata(
                                                                                                                    default
                                                                                                                    (
                                                                                                                        Brush
                                                                                                                    )));

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
                                                                                                        "StrokeThickness",
                                                                                                        typeof(double),
                                                                                                        typeof(
                                                                                                            PathButton),
                                                                                                        new
                                                                                                        PropertyMetadata
                                                                                                        (default(double
                                                                                                         )));

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
                                                                                                "Stretch",
                                                                                                typeof(Stretch),
                                                                                                typeof(PathButton),
                                                                                                new PropertyMetadata(
                                                                                                                     default
                                                                                                                     (
                                                                                                                         Stretch
                                                                                                                     )))
        ;

        public Geometry Data {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public Brush Fill {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public Brush Stroke {
            get => (Brush)GetValue(StrokeProperty);
            set => SetValue(StrokeProperty, value);
        }

        public double StrokeThickness {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        public Stretch Stretch {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public PathButton() {
            InitializeComponent();
        }
    }
}
