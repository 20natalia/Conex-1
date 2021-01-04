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

// borrowed code for the touch screen feature, except where indicated elsewise
 namespace Conex1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class DrawingPage : ContentPage
    {

        // paths being drawn by more than one figher, key = touch ID accompanies each touch event
        Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();

      

        private class Circle {
            public SKPoint center;
            public bool fill;
            public Circle(SKPoint c, bool f = false) { center = c; fill = f; }
           
        };

        // paths finished/ the finger was lifted 
        List<SKPath> completedPaths = new List<SKPath>();
        List<Circle> circleList;
        int radius = 0;

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        SKPaint paintAfter = new SKPaint
        {
            Style = SKPaintStyle.StrokeAndFill,
            Color = SKColors.Blue,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        SKPaint paintNumInit = new SKPaint
        {
            Style = SKPaintStyle.StrokeAndFill,
            Color = SKColors.Black,
            TextSize = 64,
            TextAlign = SKTextAlign.Center
                    

        };

        SKPaint paintNumAfter = new SKPaint
        {
            Style = SKPaintStyle.StrokeAndFill,
            Color = SKColors.WhiteSmoke,
            TextSize = 64,
            TextAlign = SKTextAlign.Center

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
                        // adding index of circle which was touched/passed
                        SKPoint loc = ConvertToPixel(args.Location);
                        foreach(Circle c in circleList) {
                        //for (int i=0; i<circleList.Count(); ++i) {
                            //Circle c = circleList[i];
                            if (SKPoint.Distance(loc, c.center) < radius)
                            {
                                c.fill = true;
                                break;
                            }
                            
                        }
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

        // drawing circles randomly at the start
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            int width = args.Info.Width;
            int height = args.Info.Height;
            int r = radius = Math.Min(width, height) / 24;

            if (circleList == null)
            {
                Random rand2 = new Random();
                circleList = new List<Circle>();
                while (circleList.Count() < 20)
                {
                    SKPoint p = new SKPoint();
                    p.X = rand2.Next(r, width-r);
                    p.Y = rand2.Next(r, height-r);
                    

                    bool toClose = false;
                    
                    foreach (Circle pi in circleList)
                    {
                        if (SKPoint.Distance(p, pi.center) > 2 * r) continue;
                        toClose = true;
                        break;
                    }
                    if (!toClose)
                    {
                        circleList.Add(new Circle(p));
                    }
                }
            }

            // continuously drawing the same circles underneath the lines as a part of the background
            int num = 1;
            foreach (Circle p in circleList)
            {
                canvas.DrawCircle(p.center, r, p.fill ? paintAfter : paint);
                Circle newP = new Circle (p.center, p.fill);
                newP.center.Y += 25;
                canvas.DrawText(num+ "", newP.center, newP.fill ? paintNumAfter : paintNumInit);
                num += 1;

            }

            foreach (SKPath path in completedPaths)
            {
                canvas.DrawPath(path, paint);
            }

            foreach (SKPath path in inProgressPaths.Values)
            {
                canvas.DrawPath(path, paint);
            }
        }

        /*
        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.CornflowerBlue);

            int width = e.Info.Width;
            int height = e.Info.Height;

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 25
            };

             canvas.DrawCircle(width / 2, height / 2, 50, paint);

            /* Random rand = new Random();
            for (int i = 1; i < 20; i++)
            {
                int radious = rand.Next(5, 50);
                int cx = rand.Next(0, 100);
                int cy = rand.Next(0, 100);
            }
            */

    }
}


