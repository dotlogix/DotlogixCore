// ==================================================
// Copyright 2016(C) , DotLogix
// File:  SplitView.cs
// Author:  Alexander Schill <alexander@schillnet.de>.
// Created:  23.06.2017
// LastEdited:  06.09.2017
// ==================================================

#region
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using DotLogix.UI.Controls.SplitViewPrimitives;
#endregion

namespace DotLogix.UI.Controls {
    /// <summary>
    ///     Represents a container with two views; one view for the main content and another view that is typically used for
    ///     navigation commands.
    /// </summary>
    [TemplatePart(Name = "PaneClipRectangle", Type = typeof(RectangleGeometry))]
    [TemplatePart(Name = "LightDismissLayer", Type = typeof(Rectangle))]
    [TemplatePart(Name = "PaneRoot", Type = typeof(Border))]
    [ContentProperty("Content")]
    public class SplitView : Control {
        private Rectangle _lightDismissLayer;
        private RectangleGeometry _paneClipRectangle;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SplitView" /> class.
        /// </summary>
        public SplitView() {
            DefaultStyleKey = typeof(SplitView);

            TemplateSettings = new SplitViewTemplateSettings(this);

            Loaded += (s, args) => {
                          TemplateSettings.Update();
                          OnStateChanged(false);
                      };
            SizeChanged += SplitView_SizeChanged;
        }

        private void SplitView_SizeChanged(object sender, SizeChangedEventArgs e) {
            if(e.HeightChanged && (_paneClipRectangle != null))
                _paneClipRectangle.Rect = new Rect(0, 0, OpenPaneLength, ActualHeight);
        }

        public override void OnApplyTemplate() {
            base.OnApplyTemplate();

            _paneClipRectangle = (RectangleGeometry)GetTemplateChild("PaneClipRectangle");
            if(_paneClipRectangle != null)
                _paneClipRectangle.Rect = new Rect(0, 0, OpenPaneLength, ActualHeight);

            _lightDismissLayer = (Rectangle)GetTemplateChild("LightDismissLayer");
            if(_lightDismissLayer != null)
                _lightDismissLayer.MouseLeftButtonDown += OnLightDismiss;
        }

        private void OnLightDismiss(object sender, MouseButtonEventArgs mouseButtonEventArgs) {
            if(IsPaneOpen)
                IsPaneOpen = false;
        }

        /// <summary>
        ///     Occurs when the <see cref="SplitView" /> pane is closed.
        /// </summary>
        public event EventHandler PaneClosed;

        /// <summary>
        ///     Occurs when the <see cref="SplitView" /> pane is closing.
        /// </summary>
        public event EventHandler<SplitViewPaneClosingEventArgs> PaneClosing;

        #region CompactPaneLength
        /// <summary>
        ///     Gets or sets the width of the <see cref="SplitView" /> pane in its compact display mode.
        /// </summary>
        /// <returns>
        ///     The width of the pane in it's compact display mode. The default is 48 device-independent pixel (DIP) (defined
        ///     by the SplitViewCompactPaneThemeLength resource).
        /// </returns>
        public double CompactPaneLength {
            get { return (double)GetValue(CompactPaneLengthProperty); }
            set { SetValue(CompactPaneLengthProperty, value); }
        }

        /// <summary>
        ///     Identifies the <see cref="CompactPaneLength" /> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="CompactPaneLength" /> property.</returns>
        public static readonly DependencyProperty CompactPaneLengthProperty =
        DependencyProperty.Register("CompactPaneLength", typeof(double), typeof(SplitView),
                                    new PropertyMetadata(0d, OnMetricsChanged));
        #endregion

        #region Content
        /// <summary>
        ///     Gets or sets the contents of the main panel of a <see cref="SplitView" />.
        /// </summary>
        /// <returns>The contents of the main panel of a <see cref="SplitView" />. The default is null.</returns>
        public UIElement Content {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        ///     Identifies the <see cref="Content" /> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="Content" /> dependency property.</returns>
        public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register("Content", typeof(UIElement), typeof(SplitView), new PropertyMetadata(null));
        #endregion

        #region DisplayMode
        /// <summary>
        ///     Gets of sets a value that specifies how the pane and content areas of a <see cref="SplitView" /> are shown.
        /// </summary>
        /// <returns>
        ///     A value of the enumeration that specifies how the pane and content areas of a <see cref="SplitView" /> are
        ///     shown. The default is <see cref="SplitViewDisplayMode.Overlay" />.
        /// </returns>
        public SplitViewDisplayMode DisplayMode {
            get { return (SplitViewDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        /// <summary>
        ///     Identifies the <see cref="DisplayMode" /> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="DisplayMode" /> dependency property.</returns>
        public static readonly DependencyProperty DisplayModeProperty =
        DependencyProperty.Register("DisplayMode", typeof(SplitViewDisplayMode), typeof(SplitView),
                                    new PropertyMetadata(SplitViewDisplayMode.Overlay, OnStateChanged));
        #endregion

        #region IsPaneOpen
        /// <summary>
        ///     Gets or sets a value that specifies whether the <see cref="SplitView" /> pane is expanded to its full width.
        /// </summary>
        /// <returns>true if the pane is expanded to its full width; otherwise, false. The default is true.</returns>
        public bool IsPaneOpen {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }

        /// <summary>
        ///     Identifies the <see cref="IsPaneOpen" /> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="IsPaneOpen" /> dependency property.</returns>
        public static readonly DependencyProperty IsPaneOpenProperty =
        DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(SplitView),
                                    new PropertyMetadata(false, OnIsPaneOpenChanged));
        #endregion

        #region OpenPaneLength
        /// <summary>
        ///     Gets or sets the width of the <see cref="SplitView" /> pane when it's fully expanded.
        /// </summary>
        /// <returns>
        ///     The width of the <see cref="SplitView" /> pane when it's fully expanded. The default is 320 device-independent
        ///     pixel (DIP).
        /// </returns>
        public double OpenPaneLength {
            get { return (double)GetValue(OpenPaneLengthProperty); }
            set { SetValue(OpenPaneLengthProperty, value); }
        }

        /// <summary>
        ///     Identifies the <see cref="OpenPaneLength" /> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="OpenPaneLength" /> dependency property.</returns>
        public static readonly DependencyProperty OpenPaneLengthProperty =
        DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(SplitView),
                                    new PropertyMetadata(0d, OnMetricsChanged));
        #endregion

        #region Pane
        /// <summary>
        ///     Gets or sets the contents of the pane of a <see cref="SplitView" />.
        /// </summary>
        /// <returns>The contents of the pane of a <see cref="SplitView" />. The default is null.</returns>
        public UIElement Pane {
            get { return (UIElement)GetValue(PaneProperty); }
            set { SetValue(PaneProperty, value); }
        }

        /// <summary>
        ///     Identifies the <see cref="Pane" /> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="Pane" /> dependency property.</returns>
        public static readonly DependencyProperty PaneProperty =
        DependencyProperty.Register("Pane", typeof(UIElement), typeof(SplitView), new PropertyMetadata(null));
        #endregion

        #region PaneBackground
        /// <summary>
        ///     Gets or sets the Brush to apply to the background of the <see cref="Pane" /> area of the control.
        /// </summary>
        /// <returns>The Brush to apply to the background of the <see cref="Pane" /> area of the control.</returns>
        public Brush PaneBackground {
            get { return (Brush)GetValue(PaneBackgroundProperty); }
            set { SetValue(PaneBackgroundProperty, value); }
        }


        public static readonly DependencyProperty PaneSplitLineBrushProperty = DependencyProperty.Register(
                                                                                                           "PaneSplitLineBrush",
                                                                                                           typeof(Brush
                                                                                                           ),
                                                                                                           typeof(
                                                                                                               SplitView
                                                                                                           ),
                                                                                                           new
                                                                                                           PropertyMetadata(default
                                                                                                                            (Brush
                                                                                                                            )))
        ;

        public Brush PaneSplitLineBrush {
            get { return (Brush)GetValue(PaneSplitLineBrushProperty); }
            set { SetValue(PaneSplitLineBrushProperty, value); }
        }

        /// <summary>
        ///     Identifies the <see cref="PaneBackground" /> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="PaneBackground" /> dependency property.</returns>
        public static readonly DependencyProperty PaneBackgroundProperty =
        DependencyProperty.Register("PaneBackground", typeof(Brush), typeof(SplitView), new PropertyMetadata(null));
        #endregion

        #region PanePlacement
        /// <summary>
        ///     Gets or sets a value that specifies whether the pane is shown on the right or left side of the
        ///     <see cref="SplitView" />.
        /// </summary>
        /// <returns>
        ///     A value of the enumeration that specifies whether the pane is shown on the right or left side of the
        ///     <see cref="SplitView" />. The default is <see cref="SplitViewPanePlacement.Left" />.
        /// </returns>
        public SplitViewPanePlacement PanePlacement {
            get { return (SplitViewPanePlacement)GetValue(PanePlacementProperty); }
            set { SetValue(PanePlacementProperty, value); }
        }

        /// <summary>
        ///     Identifies the PanePlacement dependency property.
        /// </summary>
        /// <returns>The identifier for the PanePlacement dependency property.</returns>
        public static readonly DependencyProperty PanePlacementProperty =
        DependencyProperty.Register("PanePlacement", typeof(SplitViewPanePlacement), typeof(SplitView),
                                    new PropertyMetadata(SplitViewPanePlacement.Left, OnStateChanged));
        #endregion

        #region TemplateSettings
        /// <summary>
        ///     Gets an object that provides calculated values that can be referenced as TemplateBinding sources when defining
        ///     templates for a <see cref="SplitView" /> control.
        /// </summary>
        /// <returns>An object that provides calculated values for templates.</returns>
        public SplitViewTemplateSettings TemplateSettings {
            get { return (SplitViewTemplateSettings)GetValue(TemplateSettingsProperty); }
            private set { SetValue(TemplateSettingsProperty, value); }
        }

        /// <summary>
        ///     Identifies the <see cref="TemplateSettings" /> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="TemplateSettings" /> dependency property.</returns>
        public static readonly DependencyProperty TemplateSettingsProperty =
        DependencyProperty.Register("TemplateSettings", typeof(SplitViewTemplateSettings), typeof(SplitView),
                                    new PropertyMetadata(null));
        #endregion

        #region Methods
        private static void OnMetricsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var sender = d as SplitView;
            sender?.TemplateSettings.Update();
        }

        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var sender = d as SplitView;
            sender?.OnStateChanged();
        }

        protected virtual void OnStateChanged(bool animated = true) {
            if(_paneClipRectangle != null)
                _paneClipRectangle.Rect = new Rect(0, 0, OpenPaneLength, ActualHeight);

            string state;
            if(IsPaneOpen)
                switch(DisplayMode) {
                    case SplitViewDisplayMode.Overlay:
                        state = $"OpenOverlay{PanePlacement}";
                        break;
                    case SplitViewDisplayMode.CompactInline:
                    case SplitViewDisplayMode.Inline:
                        state = $"OpenInline{PanePlacement}";
                        break;
                    case SplitViewDisplayMode.CompactOverlay:
                        state = $"OpenCompactOverlay{PanePlacement}";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            else
                switch(DisplayMode) {
                    case SplitViewDisplayMode.Overlay:
                    case SplitViewDisplayMode.Inline:
                        state = "Closed";
                        break;
                    case SplitViewDisplayMode.CompactOverlay:
                    case SplitViewDisplayMode.CompactInline:
                        state = $"ClosedCompact{PanePlacement}";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            VisualStateManager.GoToState(this, state, animated);
        }

        private static void OnIsPaneOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var sender = d as SplitView;
            var newValue = (bool)e.NewValue;
            var oldValue = (bool)e.OldValue;

            if((sender != null) && (newValue != oldValue))
                if(newValue)
                    sender.OnStateChanged();
                else
                    sender.OnIsPaneOpenChanged();
        }

        protected virtual void OnIsPaneOpenChanged() {
            var cancel = false;
            if(PaneClosing != null) {
                var args = new SplitViewPaneClosingEventArgs();
                foreach(var tmp in PaneClosing.GetInvocationList().
                Cast<EventHandler<SplitViewPaneClosingEventArgs>>()) {
                    tmp(this, args);
                    if(args.Cancel) {
                        cancel = true;
                        break;
                    }
                }
            }
            if(!cancel) {
                OnStateChanged();

                PaneClosed?.Invoke(this, EventArgs.Empty);
            } else {
                IsPaneOpen = false;
            }
        }
        #endregion
    }
}
