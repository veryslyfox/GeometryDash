using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

struct Rectangle
{
    public static Rectangle Create(int x, int y, int width, int height)
    {
        return new(x, y, x + width, y + height);
    }
    public Rectangle(int x1, int x2, int y1, int y2)
    {
        X1 = x1;
        X2 = x2;
        Y1 = y1;
        Y2 = y2;
    }

    public int X1 { get; }
    public int Y1 { get; }
    public int Width { get; }
    public int Height { get; }
    public int X2 { get; }
    public int Y2 { get; }
    public static Rectangle? operator &(Rectangle left, Rectangle right)
    {
        var x1 = 0;
        var x2 = 0;
        var v1 = left.X1 < right.X2;
        var v2 = left.X2 < right.X1;
        if (v1)
        {
            if (v2)
            {
                return null;
            }
            else
            {
                x1 = right.X2;
                x2 = left.X1;
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
                return null;
            }
        }
        var y1 = 0;
        var y2 = 0;
        var v3 = left.X1 < right.X2;
        var v4 = left.X2 < right.X1;
        if (v3)
        {
            if (v4)
            {
                return null;
            }
            else
            {
                y1 = right.Y2;
                y2 = left.Y1;
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
                return null;
            }
        }
        return new Rectangle(x1, x2, y1, y2);
    }
    public bool In(int x, int y)
    {
        return x < X2 & x > X1 & y < Y2 & y > Y1;
    }
}
class Graphic
{
    public Graphic(WriteableBitmap bitmap)
    {
        Bitmap = bitmap;
        InitPointers();

    }
    void InitPointers()
    {
        Pointers = new int[Bitmap.PixelHeight];
        for (int i = 0; i < Bitmap.PixelHeight; i++)
        {
            Pointers[i] = Bitmap.BackBufferStride * i;
        }
    }
    int[] Pointers = null!;
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
    public void DrawRectangle(Rectangle rectangle, int color)
    {
        for (int y = rectangle.Y1; y < rectangle.Y2; y++)
        {
            for (int x = rectangle.X1; x < rectangle.X2; x++)
            {
                var ptr = Bitmap.BackBuffer + x * 4 + Pointers[y];
                unsafe
                {
                    *((int*)ptr) = color;
                }
            }
        }
    }
}