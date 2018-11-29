﻿using System.Windows;
using System.Windows.Media;
using Biaui.Internals;

namespace Biaui.Controls
{
    public class BiaCheckBox : BiaToggleButton
    {
        #region BoxBorderColor

        public Color BoxBorderColor
        {
            get => _boxBorderColor;
            set
            {
                if (value != _boxBorderColor)
                    SetValue(BoxBorderColorProperty, value);
            }
        }

        private Color _boxBorderColor = Colors.Transparent;

        public static readonly DependencyProperty BoxBorderColorProperty =
            DependencyProperty.Register(nameof(BoxBorderColor), typeof(Color), typeof(BiaCheckBox),
                new PropertyMetadata(
                    Boxes.ColorTransparent,
                    (s, e) =>
                    {
                        var self = (BiaCheckBox) s;
                        self._boxBorderColor = (Color) e.NewValue;
                        self.InvalidateVisual();
                    }));

        #endregion

        #region MarkBrush

        public Brush MarkBrush
        {
            get => _MarkBrush;
            set
            {
                if (value != _MarkBrush)
                    SetValue(MarkBrushProperty, value);
            }
        }

        private Brush _MarkBrush = default(Brush);

        public static readonly DependencyProperty MarkBrushProperty =
            DependencyProperty.Register(nameof(MarkBrush), typeof(Brush), typeof(BiaCheckBox),
                new PropertyMetadata(
                    default(Brush),
                    (s, e) =>
                    {
                        var self = (BiaCheckBox) s;
                        self._MarkBrush = (Brush) e.NewValue;
                        self.InvalidateVisual();
                    }));

        #endregion

        static BiaCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BiaCheckBox),
                new FrameworkPropertyMetadata(typeof(BiaCheckBox)));
        }

        protected override void OnRender(DrawingContext dc)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (ActualWidth <= 1 ||
                ActualHeight <= 1)
                return;
            // ReSharper restore CompareOfFloatsByEqualityOperator

            dc.DrawRectangle(Brushes.Transparent, null, ActualRectangle);

            var rect = new Rect(0.5, 0.5, ActualWidth - 1, ActualHeight - 1);
            dc.PushGuidelineSet(Caches.GetGuidelineSet(rect, 0));
            {
                // ボックス
                dc.DrawRoundedRectangle(
                    Background,
                    Caches.GetBorderPen(BoxBorderColor, 2 / WpfHelper.PixelsPerDip),
                    new Rect(0, 2, 16, 16), null,
                    CornerRadius, null,
                    CornerRadius, null);

                // マーク
                if (IsChecked && IsPressed == false)
                {
                    dc.PushTransform(_markOffset);
                    dc.DrawGeometry(MarkBrush, null, _markGeom);
                    dc.Pop();
                }

                // キャプション
                TextRenderer.Default.Draw(Content, 16 + 4, 2, Foreground, dc, ActualWidth, TextAlignment.Left);
            }
            dc.Pop();
        }

        private Rect ActualRectangle => new Rect(new Size(ActualWidth, ActualHeight));

        private static readonly Geometry _markGeom =
            Geometry.Parse(
                "F1 M 9.97498,1.22334L 4.6983,9.09834L 4.52164,9.09834L 0,5.19331L 1.27664,3.52165L 4.255,6.08833L 8.33331,1.52588e-005L 9.97498,1.22334 Z ");

        private static readonly TranslateTransform _markOffset = new TranslateTransform(3, 5);
    }
}