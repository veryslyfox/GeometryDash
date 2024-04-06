using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows;
struct Int32Point
{
    public Int32Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    public static Int32Point operator -(Int32Point a, Int32Point b)
    {
        return new(a.X - b.X, a.Y - b.Y);
    }
    public static Int32Point operator +(Int32Point a, Int32Point b)
    {
        return new(a.X + b.X, a.Y + b.Y);
    }
    public int X { get; }
    public int Y { get; }
}
struct Triangle
{
    public Triangle(Int32Point a, Int32Point b, Int32Point c, byte red, byte green, byte blue)
    {
        A = a;
        B = b;
        C = c;
        Red = red;
        Green = green;
        Blue = blue;
    }

    public Int32Point A { get; }
    public Int32Point B { get; }
    public Int32Point C { get; }
    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }
}
#pragma warning disable
struct Rectangle
#pragma warning enable
{
    public static Rectangle Create(int x, int y, int width, int height)
    {
        return new(x, y, x + width, y + height);
    }
    public Rectangle(int x1, int y1, int x2, int y2)
    {
        X1 = x1;
        X2 = x2;
        Y1 = y1;
        Y2 = y2;
    }

    public int X1 { get; set; }
    public int Y1 { get; set; }
    public int X2 { get; set; }
    public int Y2 { get; set; }
    public static explicit operator Rectangle(FloatRectangle rectangle)
    {
        return new((int)rectangle.X1, (int)rectangle.Y1, (int)rectangle.X2, (int)rectangle.Y2);
    }
    public static Rectangle operator &(Rectangle left, Rectangle right)
    {
        var x = GetIntersection(left.X1, left.X2, right.X1, right.X2);
        var y = GetIntersection(left.Y1, left.Y2, right.Y1, right.Y2);
        return new(x.Item1, y.Item1, x.Item2, y.Item2);
    }
    public static (int, int) GetIntersection(int a1, int a2, int b1, int b2)
    {
        if (a1 > b1)
        {
            (a1, a2, b1, b2) = (b1, b2, a1, a2);
        }
        if (a2 <= b1)
        {
            return (0, 0);
        }        
        if(a1 <= b1 && a2 >= b2)
        {
            return (b1, b2);
        }
        return (b1, a2);
    }
    
    enum IntersectionType
    {
        None,
        Contain,
        Partial
    }
    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return (left.X1 == right.X1) &
        (left.X2 == right.X2) &
        (left.Y1 == right.Y1) &
        (left.Y2 == right.Y2);
    }
    public static bool operator !=(Rectangle left, Rectangle right)
    {
        return !(left == right);
    }
    public bool In(int x, int y)
    {
        return x <= X2 & x >= X1 & y <= Y2 & y >= Y1;
    }
    public Rectangle Move(int x, int y)
    {
        return new(X1 + x, Y1 + y, X2 + x, Y2 + y);
    }
}
struct FloatRectangle
{
    public FloatRectangle(double x1, double y1, double x2, double y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    public double X1 { get; set; }
    public double X2 { get; set; }
    public double Y1 { get; set; }
    public double Y2 { get; set; }
    public FloatRectangle Move(double x, double y)
    {
        return new(X1 + x, Y1 + y, X2 + x, Y2 + y);
    }
    public static FloatRectangle operator &(FloatRectangle left, FloatRectangle right)
    {
        var x1 = 0.0;
        var x2 = 0.0;
        var v1 = left.X1 < right.X2;
        var v2 = left.X2 < right.X1;
        if (v1)
        {
            if (v2)
            {
                return default;
            }
            else
            {
                x1 = right.X1;
                x2 = left.X2;
            }
        }
        else
        {
            if (v2)
            {
                x1 = left.X1;
                x2 = right.X2;
            }
            else
            {
                return default;
            }
        }
        if ((left.X1 < right.X1) & (left.X2 > right.X2))
        {
            x1 = right.X1;
            x2 = right.X2;
        }
        if ((left.X1 > right.X1) & (left.X2 < right.X2))
        {
            x1 = left.X1;
            x2 = left.X2;
        }
        var y1 = 0.0;
        var y2 = 0.0;
        var v3 = left.Y1 < right.Y2;
        var v4 = left.Y2 < right.Y1;
        if (v3)
        {
            if (v4)
            {
                return default;
            }
            else
            {
                y1 = right.Y1;
                y2 = left.Y2;
            }
        }
        else
        {
            if (v4)
            {
                y1 = left.Y1;
                y2 = right.Y2;
            }
            else
            {
                return default;
            }
        }
        if ((left.Y1 < right.Y1) & (left.Y2 > right.Y2))
        {
            y1 = right.Y1;
            y2 = right.Y2;
        }
        if ((left.Y1 > right.Y1) & (left.Y2 < right.Y2))
        {
            y1 = left.Y1;
            y2 = left.Y2;
        }
        return new(x1, y1, x2, y2);
    }
    public static bool IsCollide(FloatRectangle left, FloatRectangle right)
    {
        return ((left.X1 < right.X2) == (left.X1 > right.X2)) & ((left.Y1 < right.Y2) == (left.Y2 > right.Y1));
    }
}
class Graphic
{
    public Graphic(WriteableBitmap bitmap)
    {
        Bitmap = bitmap;
    }
    public WriteableBitmap Bitmap { get; }
    public unsafe void DrawRectangle(Rectangle rectangle, byte r, byte g, byte b)
    {
        var color = (r << 16) | (g << 8) | b;
        for (int y = rectangle.Y1; y < rectangle.Y2; y++)
        {
            for (int* ptr = (int*)(Bitmap.BackBuffer + Bitmap.BackBufferStride * y) + rectangle.X1, eptr = ptr - rectangle.X1 + rectangle.X2;
             ptr != eptr;
              ++ptr)
            {
                *ptr = color;
            }
        }
    }
    public void SafeDrawRectangle(Rectangle rectangle, byte r, byte g, byte b)
    {
        var bitmapRectangle = new Rectangle(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight);
        var newRectangle = rectangle & bitmapRectangle;
        if (newRectangle == null)
        {
            return;
        }
        DrawRectangle(newRectangle, r, g, b);
    }
    public unsafe void DrawTopTriangle(Int32Point basePoint, double delta1, double delta2, int triangleHeight, byte r, byte g, byte b)
    {
        var color = (r << 16) | (g << 8) | b;
        if (delta1 > delta2)
        {
            (delta1, delta2) = (delta2, delta1);
        }
        var rowStart = (double)basePoint.X;
        var rowEnd = (double)basePoint.X;
        for (int y = basePoint.Y; y <= basePoint.Y + triangleHeight; y++)
        {
            for (int x = (int)rowStart; x < (int)rowEnd; x++)
            {
                var ptr = x * 4 + Bitmap.BackBuffer + Bitmap.BackBufferStride * y;
                *(int*)ptr = color;
            }
            rowStart += delta1;
            rowEnd += delta2;
        }
    }
    public unsafe void DrawCircle(int xCenter, int yCenter, int radius, byte r, byte g, byte b)
    {
        var color = (r << 16) | (g << 8) | b;
        for (int y = yCenter - radius; y < yCenter + radius; y++)
        {
            for (int x = xCenter - radius; x < xCenter + radius; x++)
            {
                if ((x - xCenter) * (x - xCenter) + (y - yCenter) * (y - yCenter) < radius * radius)
                {
                    var ptr = x * 4 + Bitmap.BackBuffer + Bitmap.BackBufferStride * y;
                    *(int*)ptr = color;
                }
            }
        }
    }
    public unsafe void DrawBottomTriangle(Int32Point basePoint, double delta1, double delta2, int triangleHeight, byte r, byte g, byte b)
    {
        var color = (r << 16) | (g << 8) | b;
        delta1 = -delta1;
        delta2 = -delta2;
        if (delta1 > delta2)
        {
            (delta1, delta2) = (delta2, delta1);
        }
        var rowStart = (double)basePoint.X;
        var rowEnd = (double)basePoint.X;
        for (int y = basePoint.Y; y > basePoint.Y - triangleHeight; y--)
        {
            for (int x = (int)rowStart; x < (int)rowEnd; x++)
            {
                var ptr = x * 4 + Bitmap.BackBuffer + Bitmap.BackBufferStride * y;
                *(int*)ptr = color;
            }
            rowStart += delta1;
            rowEnd += delta2;
        }
    }
    public unsafe void DrawTriangle(Triangle triangle, byte r, byte g, byte b)
    {
        double GetDelta(Int32Point point)
        {
            return (double)point.X / point.Y;
        }
        var ay = triangle.A.Y;
        var by = triangle.B.Y;
        var cy = triangle.C.Y;
        var upper = triangle.A;
        var middle = triangle.B;
        var lower = triangle.C;
        var uy = upper.Y;
        var my = middle.Y;
        var ly = lower.Y;
        if (uy > my)
        {
            (uy, my) = (my, uy);
            (upper, middle) = (middle, upper);
        }
        if (my > ly)
        {
            (my, ly) = (ly, my);
            (middle, lower) = (lower, middle);
        }
        if (uy > my)
        {
            (uy, my) = (my, uy);
            (upper, middle) = (middle, upper);
        }
        if (my > ly)
        {
            (my, ly) = (ly, my);
            (middle, lower) = (lower, middle);
        }
        var topTriangleHeight = (middle.Y - upper.Y);
        var bottomTriangleHeight = (lower.Y - middle.Y);
        if (topTriangleHeight != 0)
        {
            DrawTopTriangle(upper, GetDelta(middle - upper), GetDelta(lower - upper), topTriangleHeight, r, g, b);
        }
        if (bottomTriangleHeight != 0)
        {
            DrawBottomTriangle(lower, GetDelta(middle - lower), GetDelta(upper - lower), bottomTriangleHeight, r, g, b);
        }
    }
    public unsafe void Clear()
    {
        NativeMemory.Fill((void*)(Bitmap.BackBuffer), (nuint)(Bitmap.BackBufferStride * Bitmap.PixelHeight), 0);
    }
    public void Fill(byte r, byte g, byte b)
    {
        DrawRectangle(new Rectangle(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight), r, g, b);
    }
}
class Camera
{
    public Camera(double xOffset, double yOffset)
    {
        XOffset = xOffset;
        YOffset = yOffset;
    }

    public double XOffset { get; }
    public double YOffset { get; }
}