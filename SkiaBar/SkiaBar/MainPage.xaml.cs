using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkiaBar
{
    public partial class MainPage : ContentPage
    {
        private SKGLView ViewDraw { get; set; }
        private Slider Sld { get; set; }
        public MainPage()
        {
            InitializeComponent();

            Title = "Skia";
            BackgroundColor = Color.White;

            ViewDraw = new SKGLView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 40,
                EnableTouchEvents= true
            };
            ViewDraw.PaintSurface += CanvasPaintSurface;

            //ViewDraw.Touch += (sender, args) =>
            //{
            //    var pt = args.Location;

            //    switch (args.ActionType)
            //    {
            //        case SKTouchAction.Pressed:
            //            ViewDraw.InvalidateSurface();
            //            break;
            //        case SKTouchAction.Moved:
            //            ViewDraw.InvalidateSurface();
            //            break;
            //    }
            //    // Let the OS know that we want to receive more touch events
            //    args.Handled = true;
            //};
            //ViewDraw.InputTransparent = false;

            Sld = new Slider
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Minimum = 0f,
                Maximum = 1f,
                Value = 0.5f
            };
            Sld.ValueChanged += SldValueChanged;

            var LayoutPage = new StackLayout
            {
                Padding = 10,
                Spacing = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    ViewDraw,
                    Sld
                }
            };

            Content = LayoutPage;
        }

        private void SldValueChanged(object sender, ValueChangedEventArgs e)
        {
            ViewDraw.InvalidateSurface();
        }

        private void CanvasPaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var rect = e.BackendRenderTarget.Rect;
            var radius = rect.Height / 2;

            canvas.Clear(SKColors.White);

            var ProgessColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColor.FromHsl(202f, 78.3f, 45.1f),
                StrokeWidth = 1
            };

            var BgColor = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
                Color = SKColor.FromHsl(0f, 0f, 93.3f),
                StrokeWidth = 1
            };

            var TextColor = new SKPaint
            {
                IsAntialias = true,
                FakeBoldText = true,
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black,
                TextSize = radius,
                TextAlign = SKTextAlign.Left
            };


            canvas.DrawRect(new SKRect(radius, 0, rect.Width - radius, rect.Height), BgColor);
            canvas.DrawCircle(radius, radius, radius, BgColor);
            canvas.DrawCircle(rect.Width - radius, radius, radius, BgColor);

            var width = rect.Width - 2 * radius;

            var val = (int)(width * Sld.Value);

            canvas.DrawRect(new SKRect(radius, 0, radius + val, rect.Height), ProgessColor);
            canvas.DrawCircle(radius, radius, radius, ProgessColor);
            canvas.DrawCircle(radius + val, radius, radius, ProgessColor);

            canvas.DrawText(string.Format("${0:0.00}", 100 * Sld.Value), radius, 1.5f * radius, TextColor);

            ProgessColor.Dispose();
            BgColor.Dispose();
            TextColor.Dispose();
        }


    }
}
