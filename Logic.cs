class Player
{
    public Player(double x, double y, FloatRectangle hitbox)
    {
        X = x;
        Y = y;
        Hitbox = hitbox;
    }
    public void Collide()
    {
        Hitbox.Move(X, Y);
        
    }
    public double X { get; set; }
    public double Y { get; set; }
    public FloatRectangle Hitbox { get; }
    public bool IsOnBlock { get; }
}
class Map
{
    public Map(GameObject[] objects)
    {
        Objects = objects;
    }

    public GameObject[] Objects { get; }
}
struct GameObject
{
    public GameObject(Rectangle rectangle, byte r, byte g, byte b, GameObjectType type)
    {
        Rectangle = rectangle;
        R = r;
        G = g;
        B = b;
        Type = type;
    }

    public Rectangle Rectangle { get; }
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }
    public GameObjectType Type { get; }
}
enum GameObjectType
{
    Block,
    Spike
}