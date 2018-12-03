﻿using System.Windows;
using System.Windows.Media;
using Biaui.Internals;

namespace Biaui.Controls
{
    using static FrameworkElementHelper;

    public class BiaHsvBoxCursor : FrameworkElement
    {
        #region BorderColor

        public Color BorderColor
        {
            get => _BorderColor;
            set
            {
                if (value != _BorderColor)
                    SetValue(BorderColorProperty, value);
            }
        }

        private Color _BorderColor = Colors.Red;

        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register(nameof(BorderColor), typeof(Color), typeof(BiaHsvBoxCursor),
                new FrameworkPropertyMetadata(
                    Boxes.ColorRed,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                    (s, e) =>
                    {
                        var self = (BiaHsvBoxCursor) s;
                        self._BorderColor = (Color) e.NewValue;
                    }));

        #endregion

        #region Hue

        public double Hue
        {
            get => _Hue;
            set
            {
                if (NumberHelper.AreClose(value, _Hue) == false)
                    SetValue(HueProperty, value);
            }
        }

        private double _Hue;

        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register(nameof(Hue), typeof(double), typeof(BiaHsvBoxCursor),
                new FrameworkPropertyMetadata(
                    Boxes.Double0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                    (s, e) =>
                    {
                        var self = (BiaHsvBoxCursor) s;
                        self._Hue = (double) e.NewValue;
                    }));

        #endregion

        #region Saturation

        public double Saturation
        {
            get => _Saturation;
            set
            {
                if (NumberHelper.AreClose(value, _Saturation) == false)
                    SetValue(SaturationProperty, value);
            }
        }

        private double _Saturation;

        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(BiaHsvBoxCursor),
                new FrameworkPropertyMetadata(
                    Boxes.Double0,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                    (s, e) =>
                    {
                        var self = (BiaHsvBoxCursor) s;
                        self._Saturation = (double) e.NewValue;
                    }));

        #endregion

        #region IsReadOnly

        public bool IsReadOnly
        {
            get => _IsReadOnly;
            set
            {
                if (value != _IsReadOnly)
                    SetValue(IsReadOnlyProperty, Boxes.Bool(value));
            }
        }

        private bool _IsReadOnly = default(bool);

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(BiaHsvBoxCursor),
                new FrameworkPropertyMetadata(
                    Boxes.BoolFalse,
                    FrameworkPropertyMetadataOptions.AffectsRender |
                    FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                    (s, e) =>
                    {
                        var self = (BiaHsvBoxCursor) s;
                        self._IsReadOnly = (bool) e.NewValue;
                    }));

        #endregion

        public BiaHsvBoxCursor()
        {
            IsHitTestVisible = false;
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (ActualWidth <= 1 ||
                ActualHeight <= 1)
                return;

            // Border
            {
                var p = this.GetBorderPen(BorderColor);

                var w = RoundLayoutValue(ActualWidth - 0.5);
                var h = RoundLayoutValue(ActualHeight - 0.5);
                var z = RoundLayoutValue(0.5);

                var p0 = new Point(z, z);
                var p1 = new Point(w, z);
                var p2 = new Point(z, h);
                var p3 = new Point(w, h);

                var p0a = new Point(z - 0.5, z);
                var p1a = new Point(w + 0.5, z);
                var p2a = new Point(z - 0.5, h);
                var p3a = new Point(w + 0.5, h);

                dc.DrawLine(p, p0a, p1a);
                dc.DrawLine(p, p1, p3);
                dc.DrawLine(p, p3a, p2a);
                dc.DrawLine(p, p2, p0);
            }

            // Cursor
            RenderHelper.DrawPointCursor(dc, CursorRenderPos, IsEnabled, IsReadOnly);
        }

        private Point CursorRenderPos
        {
            get
            {
                var bw = RoundLayoutValue(FrameworkElementExtensions.BorderWidth * 2);
                var w = RoundLayoutValue(ActualWidth - bw * 2);
                var h = RoundLayoutValue(ActualHeight - bw * 2);

                var x = Hue * w + bw;
                var y = (1 - Saturation) * h + bw;

                return new Point(RoundLayoutValue(x), RoundLayoutValue(y));
            }
        }
    }
}