class Player
{
    public Player(double x, double y, Rectangle hitbox)
    {
        X = x;
        Y = y;
        Hitbox = hitbox;
    }
    
    public double X { get; set; }
    public double Y { get; set; }
    public Rectangle Hitbox { get; }
}
class Map
{
    public Map()
    {

    }
}
struct GameObject
{
    public GameObject(Rectangle rectangle, byte r, byte g, byte b)
    {
        
    }
}
enum GameObjectType
{
    
}