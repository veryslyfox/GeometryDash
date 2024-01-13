class Player
{
    public Player(double x, double y, FloatRectangle hitbox)
    {
        X = x;
        Y = y;
        Hitbox = hitbox;
    }
    public void Kill()
    {
        X = 0;
        Y = 500;
    }
    public void Tick()
    {
        X += Speed;
        if (IsOnBlock)
        {
            Move = 0;
        }
        Y += Move;
        Move -= Gravity;
    }
    public double X { get; set; }
    public double Y { get; set; }
    public double Move { get; set; }
    //TEST_ME
    public double Speed { get; set; }
    public double Gravity { get; set; }
    public FloatRectangle Hitbox { get; set; }
    public bool IsOnBlock { get; set; }
}
class Map
{
    public Map(GameObject[] objects)
    {
        Objects = objects;
    }
    public void Activate(Player player)
    {
        foreach (var gameObject in Objects)
        {
            if (FloatRectangle.IsCollide(gameObject.Rectangle, player.Hitbox))
            {
                switch (gameObject.Type)
                {
                    case GameObjectType.None:
                        break;
                    case GameObjectType.Block:
                        player.IsOnBlock = true;
                        break;
                    case GameObjectType.Spike:
                        player.Kill();
                        break;
                    default:
                        if (Test.Value)
                        {
                            throw new InvalidDataException("invalid object type");
                        }
                        break;
                }
            }
        }
    }
    public GameObject[] Objects { get; }
}
struct GameObject
{
    public GameObject(FloatRectangle rectangle, byte r, byte g, byte b, GameObjectType type)
    {
        Rectangle = rectangle;
        R = r;
        G = g;
        B = b;
        Type = type;
    }
    public FloatRectangle Rectangle { get; }
    public byte R { get; }
    public byte G { get; }
    public byte B { get; }
    public GameObjectType Type { get; }
}
enum GameObjectType
{
    None,
    Block,
    Spike
}