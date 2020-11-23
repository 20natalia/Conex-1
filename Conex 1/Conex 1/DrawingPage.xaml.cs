using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TouchTracking;

 namespace Conex1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class DrawingPage : ContentPage
    {

        // paths being drawn by more than one figher, key = touch ID accompanies each touch event
        Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
        
        // paths finished/ the finger was lifted 
        List<SKPath> completedPaths = new List<SKPath>();

        
        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        public DrawingPage()
        {
            InitializeComponent();
        }


        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = new SKPath();
                        path.MoveTo(ConvertToPixel(args.Location));
                        inProgressPaths.Add(args.Id, path);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Moved:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = inProgressPaths[args.Id];
                        path.LineTo(ConvertToPixel(args.Location));
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        // My Random Color Implementation
                        Random rand = new Random();
                        Byte[] rgb = new Byte[3];
                        rand.NextBytes(rgb);
                        paint.Color = new SKColor(rgb[0], rgb[1], rgb[2]);

                        completedPaths.Add(inProgressPaths[args.Id]);
                        inProgressPaths.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Cancelled:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        inProgressPaths.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
        }
   
         SKPoint ConvertToPixel(Point pt)
         {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
         }

        SKPoint ConvertToPixel(TouchTrackingPoint pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            foreach (SKPath path in completedPaths)
            {
                canvas.DrawPath(path, paint);

              
            }
            foreach (SKPath path in inProgressPaths.Values)
            {
                canvas.DrawPath(path, paint);

                if(completedPaths.Contains(path))
                {
                    canvas.DrawCircle(path.LastPoint.X, path.LastPoint.Y, 50, paint);
                }
            }
        }
    }
}


