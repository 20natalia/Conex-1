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
using System.Diagnostics;

// borrowed code for the touch screen feature, except where indicated elsewise
namespace Conex1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class DrawingPage : ContentPage
    {

        // paths being drawn by more than one figher, key = touch ID accompanies each touch event
        SKPath inProgressPath;    

        private class Circle {
            public SKPoint center;
            public bool fill;
            public Circle(SKPoint c, bool f = false) { center = c; fill = f; }
        };

        private class Completed
        {
            public SKPath path;
            public int player;
            public Completed(SKPath p, int k) { path = p; player = k; }
        };

     

        // paths finished/ the finger was lifted 
        List<Completed> completedPaths;
        List<Circle> circleList;
        List<Circle> circleIntersections = new List<Circle>();
        int radius = 0;
        Boolean turn = true;
        int numCircles;
        SKCanvas canvas;
        long ticks = 0;
       

        //endGame Views, B
    
        //bool isAnimating;
        Stopwatch stopwatch = new Stopwatch();
        //double transparency;
        
       
        

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        SKPaint paintPlayer1 = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        SKPaint paintPlayer2 = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.PaleVioletRed,
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

        private WelcomePage main;

        public DrawingPage(WelcomePage m)
        {
            main = m;
            numCircles = m.gID;
            InitializeComponent();
        }

        private bool lineCross(SKPoint loc, SKPoint vec, SKPoint c)
        {
            if (SKPoint.Distance(loc, c) < radius) return true;
            SKPoint vc = c - loc;
            float d = vc.X * vec.X + vc.Y * vec.Y;
            if (d < 0) return false;
            float v = vec.LengthSquared;
            if (d >= v) return false;
            float s = vc.X * vec.Y - vc.Y * vec.X;
            if (s * s < radius * radius * v) return true; 
            return false;
        }

        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            SKPoint point = ConvertToPixel(args.Location);
            OnTouchEffectAction(args.Type, point);
        }

        private void OnTouchEffectAction(TouchActionType type, SKPoint point)
        {
            switch (type)
            {
                case TouchActionType.Pressed:
                    if (ticks == 0)
                    {
                        ticks = DateTime.Now.Ticks;
                        Device.StartTimer(TimeSpan.FromMilliseconds(20), () =>
                        {
                            canvasView.InvalidateSurface();
                            return ticks != 0;
                        });
                    }
                    
                    if (completedPaths != null)
                    {
                        Completed last = completedPaths.Last();
                        if (SKPoint.Distance(last.path.LastPoint, point) > radius / 2) { break; }
                    }
                    inProgressPath = new SKPath();
                    inProgressPath.MoveTo(point);
                    canvasView.InvalidateSurface();
                    break;

                case TouchActionType.Moved:
                    if (inProgressPath == null) break;
                    inProgressPath.LineTo(point);
                    // adding index of circle which was touched/passed
                    SKPoint vec = inProgressPath.LastPoint - point;
                    bool cross = false;

                    foreach (Circle c in circleList) {
                        if (circleList.IndexOf(c) == numCircles-1 && c.fill == true)
                        {
                            if (ticks != 0)
                            {
                                double s = 0.01 * Math.Floor(0.00001 * (DateTime.Now.Ticks - ticks));
                                main.addResult(s, numCircles);
                                ticks = 0;
                            }
                            endGameAsync();
                            return;
                        }
                        if (c.fill) continue;
                        if (lineCross(point, vec, c.center))
                        {
                            int ind = circleList.IndexOf(c);
                            if (ind == 0 || circleList.ElementAt(ind - 1).fill)
                            {
                                c.fill = true;
                                OnTouchEffectAction(TouchActionType.Released, point);
                                turn = !turn;
                                OnTouchEffectAction(TouchActionType.Pressed, point);
                                cross = true;
                                break;
                            }
                            break;

                        }

                    }
                    if (!cross) {
                        canvasView.InvalidateSurface();
                    }

                    break;
                
                case TouchActionType.Released:
                    if (inProgressPath == null) break;
                    // My Random Color Implementation
                    Random rand = new Random();
                    Byte[] rgb = new Byte[3];
                    rand.NextBytes(rgb);
                    paint.Color = new SKColor(rgb[0], rgb[1], rgb[2]);
                    Completed completed = new Completed(inProgressPath, turn ? 0 : 1);
                    if (completedPaths == null) completedPaths = new List<Completed>();
                    completedPaths.Add(completed);
                    inProgressPath = null;
                    break;

                case TouchActionType.Cancelled:
                    inProgressPath = null;
                    canvasView.InvalidateSurface();
                    break;
            }
        }

     



        //B
        SKPoint ConvertToPixel(Point pt)
         {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
         }
        //B
        SKPoint ConvertToPixel(TouchTrackingPoint pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }

        // drawing circles randomly at the start
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            canvas = args.Surface.Canvas;
            canvas.Clear();

            int width = args.Info.Width;
            int height = args.Info.Height;
            int r = radius = Math.Min(width, height) / 24;
            

            if (circleList == null)
            {
                Random rand2 = new Random();
                circleList = new List<Circle>();
                while (circleList.Count() < numCircles)
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
                canvas.DrawText(circleList.IndexOf(p) + 1 + "", newP.center, newP.fill ? paintNumAfter : paintNumInit);
                num += 1;

            }

            if (completedPaths != null)
            {
                foreach (Completed c in completedPaths)
                {
                    canvas.DrawPath(c.path, c.player == 0 ? paintPlayer1 : paintPlayer2);
                }
            }

            if (inProgressPath != null)
            {
                canvas.DrawPath(inProgressPath, turn ? paintPlayer1 : paintPlayer2);
            }

            foreach (Circle s in circleIntersections)
            {
                canvas.DrawCircle(s.center, r / 12, paintAfter);
            }

            if (ticks != 0)
            {
                double s = 0.01 * Math.Floor(0.00001*(DateTime.Now.Ticks - ticks));
                double m = Math.Floor(s / 60);
                s -= 60 * m; 

                canvas.DrawText(m + ":" + s.ToString("00.00"), 300, 50, paintNumInit);
            }
        }

        //end Game + transparency
        private async Task endGameAsync()
        {
            await Navigation.PopAsync();
        }
    }
}


