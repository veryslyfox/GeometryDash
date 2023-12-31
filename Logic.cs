class Player
{
    public Player(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; set; }
    public double Y { get; set; }
    public Rectangle Hitbox;
}