class RectAccumulator
{
    public RectAccumulator()
    {

    }
    public void Add(FloatRectangle rectangle)
    {
        Result = new(Math.Min(rectangle.X1, Result.X1), Math.Min(rectangle.Y1, Result.Y1), Math.Max(rectangle.X2, Result.X2), Math.Max(rectangle.Y2, Result.Y2));
    
    }
    public FloatRectangle Result { get; set; }
}