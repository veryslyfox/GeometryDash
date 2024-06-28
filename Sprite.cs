class Sprite
{
    public Sprite(int[,] colors)
    {
        Colors = colors;
    }
    public int[,] Colors;
    public int X { get => Colors.GetLength(0);}
    public int Y { get => Colors.GetLength(1);}
    public int this[int x, int y]
    {
        get => Colors[x, y];
        set => Colors[x, y] = value;
    }
}