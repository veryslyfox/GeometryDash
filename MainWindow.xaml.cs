namespace _3dAppTest;
using System;
using System.Collections.Generic;
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
    private Graphic _graphic;
    public MainWindow()
    {
        InitializeComponent();
        _timer.Interval = TimeSpan.FromSeconds(0.000001);
        _bitmap = new WriteableBitmap(1000, 1000, 96, 96, PixelFormats.Bgr32, null);
        image.Source = _bitmap;
        _graphic = new Graphic(_bitmap);
        _timer.Tick += Tick;
        _timer.Start();
    }

    public void Tick(object? sender, EventArgs args)
    {
        _bitmap.Lock();
        _graphic.DrawRectangle(new Rectangle(100, 200, 100, 200), 255, 0, 0);
        _bitmap.Unlock();
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