global using System;
global using System.IO;
namespace _3dAppTest;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Rectangle = global::Rectangle;

public partial class MainWindow : Window
{
    private readonly DispatcherTimer _timer = new();
    private readonly WriteableBitmap _bitmap;
    private readonly Random _rng = new();
    private int _f;
    private int _q;
    private Graphic _graphic;
    private long _startTime;
    private FloatRectangle _rectangle = new(350, 350, 450, 450);
    private Map _map;
    private Player _player;
    public MainWindow()
    {
        InitializeComponent();
        _timer.Interval = TimeSpan.FromSeconds(0.000001);
        _bitmap = new WriteableBitmap(1000, 1000, 96, 96, PixelFormats.Bgr32, null);
        image.Source = _bitmap;
        _graphic = new Graphic(_bitmap);
        _startTime = Stopwatch.GetTimestamp();
        _timer.Tick += Tick;
        _timer.Start();
    }

    public void Tick(object? sender, EventArgs args)
    {
        _bitmap.Lock();
        _graphic.Clear();
        // DrawFractal(new(new(250, 600), new(750, 600), new(500, 166), 0, 0, 0), 9);
        _graphic.SafeDrawRectangle(new())
        foreach (var obj in _map.Objects)
        {
            _graphic.SafeDrawRectangle(new((int)obj.Rectangle.X1, (int)obj.Rectangle.Y1, (int)obj.Rectangle.X2, (int)obj.Rectangle.Y2), obj.R, obj.G, obj.B);
        }
        // _graphic.DrawRectangle(new Rectangle(_rng.Next(500), _rng.Next(500), _rng.Next(500) + 500, _rng.Next(500) + 500) & new Rectangle(0, 0, 1000, 1000), color.R, color.G, color.B);
        // _bitmap.AddDirtyRect(new Int32Rect(0, 0, 1000, 1000));
        // _bitmap.Unlock();
        // _f += _rng.Next(2) * 48 - 24;
        // _rectangle.Move(_moveX, _moveY);
        // var time = Stopwatch.GetTimestamp();
        // var diff = time - _startTime;
        // if (diff > Stopwatch.Frequency * 0.1)
        // {
        //     _startTime = time;
        //     var alpha = _rng.NextDouble() * Math.Tau;
        //     _moveX = Math.Sin(alpha) * 2;
        //     _moveY = Math.Cos(alpha) * 2;
        // }
        // _f++;
        // if (diff > Stopwatch.Frequency)
        // {
        //     var seconds = (double)diff / Stopwatch.Frequency;
        //     using var fpsWriter = new StreamWriter(File.Open("fps.txt", FileMode.Create));
        //     fpsWriter.WriteLine(_f / seconds);
        //     _startTime = time;
        //     _f = 0;
        // }
        //_f++;
        // var rect1 = Rectangle.Create(_rng.Next(900), _rng.Next(900), _rng.Next(100), _rng.Next(100));
        // var rect2 = Rectangle.Create(_rng.Next(900), _rng.Next(900), _rng.Next(100), _rng.Next(100));
        // if ((rect1 & rect2) == null)
        // {
        //     return;
        // }
        // var rect3 = (rect1 & rect2).Value;
        // _bitmap.Lock();
        // for (int y = 0; y < _bitmap.PixelWidth; y++)
        // {
        //     for (int x = 0; x < _bitmap.PixelHeight; x++)
        //     {
        //         var v = (rect1.In(x, y) ? 1 : 0) + (rect2.In(x, y) ? 1 : 0) + (rect3.In(x, y) ? 1 : 0);
        //         v = v * 80;
        //         var r = v;
        //         var g = v;
        //         var b = v;
        //         var ptr = _bitmap.BackBuffer + x * 4 + _bitmap.BackBufferStride * y;
        //         unsafe
        //         {
        //             *((int*)ptr) = (r << 16) | (g << 8) | b;
        //         }
        //     }
        // }
        _f++;
        _bitmap.AddDirtyRect(new(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight));
        _bitmap.Unlock();
    }
    void DrawFractal(Triangle triangle, int depth)
    {
        if (depth == 0)
        {
            return;
        }
        var color = HsvToRgb(depth * 30, 255, 255);
        _graphic.DrawTriangle(triangle, color.R, color.G, color.B);
        var middlePoint1 = new Int32Point((triangle.A.X + triangle.B.X) / 2, (triangle.A.Y + triangle.B.Y) / 2);
        var middlePoint2 = new Int32Point((triangle.A.X + triangle.C.X) / 2, (triangle.A.Y + triangle.C.Y) / 2);
        var middlePoint3 = new Int32Point((triangle.B.X + triangle.C.X) / 2, (triangle.B.Y + triangle.C.Y) / 2);
        DrawFractal(new(triangle.A, middlePoint1, middlePoint2, color.R, color.G, color.B), depth - 1);
        DrawFractal(new(triangle.B, middlePoint1, middlePoint3, color.R, color.G, color.B), depth - 1);
        DrawFractal(new(triangle.C, middlePoint2, middlePoint3, color.R, color.G, color.B), depth - 1);
    }
    void DrawFractal2(Rectangle rectangle, int depth)
    {
        if (depth == 0)
        {
            return;
        }
        var color = HsvToRgb(depth * 30, 255, 255);
        var x1 = rectangle.X1;
        var x2 = rectangle.X2;
        var y1 = rectangle.Y1;
        var y2 = rectangle.Y2;
        var xMin = (int)(rectangle.X1 * 0.666 + rectangle.X2 * 0.333);
        var xMax = (int)(rectangle.X1 * 0.333 + rectangle.X2 * 0.666);
        var yMin = (int)(rectangle.Y1 * 0.666 + rectangle.Y2 * 0.333);
        var yMax = (int)(rectangle.Y1 * 0.333 + rectangle.Y2 * 0.666);
        _graphic.DrawRectangle(new(xMin, xMax, yMin, yMax), color.R, color.G, color.B);
        DrawFractal2(new(x1, y1, xMin, yMin), depth - 1);
        DrawFractal2(new(xMin, y1, xMax, yMin), depth - 1);
        DrawFractal2(new(xMax, y1, x2, yMin), depth - 1);
        DrawFractal2(new(x1, yMin, xMin, yMax), depth - 1);
        DrawFractal2(new(xMax, yMin, x2, yMax), depth - 1);
        DrawFractal2(new(x1, y1, xMin, yMin), depth - 1);
        DrawFractal2(new(x1, y1, xMin, yMin), depth - 1);
        DrawFractal2(new(x1, y1, xMin, yMin), depth - 1);
    }
    public Color HsvToRgb(int h, byte s, byte v)
    {
        var result = new Color();
        var hue = h % 360;
        var hv = (hue % 60) * 255 / 60;
        var a = 0;
        var b = hv;
        var c = 255 - hv;
        var d = 255;
        void DoubleInterval(int min, int max, int r, int g, int b)
        {
            if (min <= hue && max > hue)
            {
                result = Normalize(FromRgb(r, g, b), v);
            }
        }
        DoubleInterval(0, 60, d, b, a);
        DoubleInterval(60, 120, c, d, a);
        DoubleInterval(120, 180, a, d, b);
        DoubleInterval(180, 240, a, c, d);
        DoubleInterval(240, 300, b, a, d);
        DoubleInterval(300, 360, d, a, c);
        return Interpolation(result, Color.FromRgb(v, v, v), s);
    }
    public Color Interpolation(Color a, Color b, byte c)
    {
        return FromRgb((a.R * c + b.R * (255 - c)) / 255, (a.G * c + b.G * (255 - c)) / 255, (a.B * c + b.B * (255 - c)) / 255);
    }
    private Color FromRgb(int r, int g, int b)
    {
        return Color.FromRgb(((byte)(r & 255)), ((byte)(g & 255)), ((byte)(b & 255)));
    }
    public Color Normalize(Color color, byte lightness)
    {
        var r = color.R;
        var g = color.G;
        var b = color.B;
        var max = Math.Max(r, Math.Max(g, b));
        if (max == 0)
        {
            return Color.FromRgb(lightness, lightness, lightness);
        }
        var normalizer = (double)lightness / max;
        return Color.FromRgb((byte)(r * normalizer), ((byte)(g * normalizer)), ((byte)(b * normalizer)));

    }
}
