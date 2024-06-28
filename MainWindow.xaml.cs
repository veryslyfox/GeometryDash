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
    private Graphic _graphic;
    private long _startTime;
    private FloatRectangle _rectangle = new(350, 350, 450, 450);
    private Level _level;
    private List<GameObject> _objects = new() { };
    private Player _player;
    private Camera _camera = new(0, 0);
    private Vector _cameraMove = new Vector(0, 0);
    private bool _isEditMode;
    public MainWindow()
    {
        for (int i = 0; i < 4000; i++)
        {
            _objects.Add(new GameObject(new(i * 100, _rng.Next(-100, 100), i * 100 + 100, _rng.Next(100, 300)), 255, 0, 0, GameObjectType.Danger));
            _objects.Add(new GameObject(new(i * 100, _rng.Next(700, 900), i * 100 + 100, _rng.Next(900, 1100)), 255, 0, 0, GameObjectType.Danger));
        }
        
        _level = new Level(_objects.ToArray());
        InitializeComponent();
        _timer.Interval = TimeSpan.FromSeconds(0.000001);
        _bitmap = new WriteableBitmap(1000, 1000, 96, 96, PixelFormats.Bgr32, null);
        image.Source = _bitmap;
        _graphic = new Graphic(_bitmap);
        _player = new Player(400, 400, new FloatRectangle(400, 400, 500, 500), 3, _level);
        _startTime = Stopwatch.GetTimestamp();
        _timer.Tick += Tick;
        _timer.Start();
        KeyDown += _player.KeyDown;
        KeyUp += _player.KeyUp;
        KeyDown += KeyDownHandler;
        KeyUp += KeyUpHandler;
    }
    public void KeyDownHandler(object sender, KeyEventArgs args)
    {
        switch (args.Key)
        {
            case Key.Left:
                _cameraMove = new(-1, 0);
                break;
            case Key.Right:
                _cameraMove = new(1, 0);
                break;
            case Key.Down:
                _cameraMove = new(0, 1);
                break;
            case Key.Up:
                _cameraMove = new(0, -1);
                break;
            case Key.D0:
                _objects.Add(new GameObject(new(_camera.XOffset + 400, _camera.YOffset + 400, _camera.XOffset + 500, _camera.YOffset + 500), 0, 0, 255, GameObjectType.Block));
                break;
            case Key.D1:
                _objects.Add(new GameObject(new(_camera.XOffset + 400, _camera.YOffset + 400, _camera.XOffset + 500, _camera.YOffset + 500), 255, 0, 0, GameObjectType.Danger));
                break;
            case Key.E:
                if (_isEditMode)
                {
                    _level = new(_objects.ToArray());
                }
                _isEditMode = !_isEditMode;
                break;
        }
    }
    public void KeyUpHandler(object sender, KeyEventArgs args)
    {
        switch (args.Key)
        {
            case Key.Left:
            case Key.Right:
            case Key.Down:
            case Key.Up:
                _cameraMove = new(0, 0);
                break;
        }
    }
    public unsafe void Tick(object? sender, EventArgs args)
    {
        _bitmap.Lock();
        _graphic.Clear();
        var noise = new Sprite(new int[100, 100]);
        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < 100; x++)
            {
                noise[x, y] = _rng.Next(_f * 1000);   
            }
        }
        _camera.XOffset += _cameraMove.X;
        _camera.YOffset += _cameraMove.Y;
        _player.Angle = 2;
        if (!_isEditMode)
        {
            _camera = new(_player.X - 350, _player.Y - 350);
            _level.Activate(_player);
            _player.Tick();
        }
        var time = Stopwatch.GetTimestamp();
        var diff = time - _startTime;
        _camera = new(_player.X - 350, _player.Y - 350);
        _graphic.DrawSprite(noise, ((Rectangle)_player.Hitbox.Move(-_camera.XOffset, -_camera.YOffset)).X1, ((Rectangle)_player.Hitbox.Move(-_camera.XOffset, -_camera.YOffset)).Y1);
        foreach (var obj in _objects)
        {
            _graphic.SafeDrawRectangle((Rectangle)obj.Rectangle.Move(-_camera.XOffset, -_camera.YOffset), obj.R, obj.G, obj.B);
        }
        if (diff > Stopwatch.Frequency)
        {
            var seconds = (double)diff / Stopwatch.Frequency;
            //using var fpsWriter = new StreamWriter(File.Open("fps.txt", FileMode.Create));
            //fpsWriter.WriteLine(_f / seconds);
            this.Title = $"FPS: {Math.Round(_f / seconds)} delay:{(Stopwatch.GetTimestamp() - _player.LastTickTime) / Stopwatch.Frequency} x: {_player.X} y: {_player.Y}";
            _startTime = time;
            _f = 0;
        }
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
