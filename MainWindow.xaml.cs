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
    private double _moveX;
    private double _moveY;
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
        _graphic.SafeDrawRectangle((Rectangle)_rectangle, 255, 0, 255);
        _bitmap.AddDirtyRect(new Int32Rect(0, 0, 1000, 1000));
        _bitmap.Unlock();
        _rectangle.Move(_moveX, _moveY);
        var time = Stopwatch.GetTimestamp();
        var diff = time - _startTime;
        if (diff > Stopwatch.Frequency * 0.1)
        {
            _startTime = time;
            var alpha = _rng.NextDouble() * Math.Tau;
            _moveX = Math.Sin(alpha) * 2;
            _moveY = Math.Cos(alpha) * 2;
        }
        _f++;
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
        // _f++;
        // _bitmap.AddDirtyRect(new(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight));
        // _bitmap.Unlock();
    }
}