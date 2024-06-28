class RectAccumulator
{
    public RectAccumulator()
    {

    }
    public void Add(Rectangle rectangle)
    {
        Result.X1 = Math.Min(Result.X1, rectangle.X1);
    
    }
    public Rectangle Result { get; set; }
}