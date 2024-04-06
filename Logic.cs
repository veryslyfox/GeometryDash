using System.Collections;
using System.Windows.Input;
class Player
{
    public Player(double x, double y, FloatRectangle hitbox, double move)
    {
        X = x;
        Y = y;
        Hitbox = hitbox;
        Move = move;
    }

    public void KeyDown(object sender, KeyEventArgs args)
    {
        if (args.Key != Key.Up)
        {
            return;
        }
        Move = -3;
        IsOnBlock = false;
    }
    internal void KeyUp(object sender, KeyEventArgs args)
    {
        Move = 3;
        return;
    }
    public void Kill()
    {
        Hitbox = Hitbox.Move(-X, -Y);
        X = 0;
        Y = 0;
    }
    public void Tick()
    {
        if (X > 800)
        {
            Speed = -Math.Abs(Speed);
        }
        if (X < 0)
        {
            Speed = Math.Abs(Speed);
        }
        Hitbox = Hitbox.Move(Speed, Move);
        X += Speed;
        if (IsOnBlock)
        {
            Move = 0;
        }
        Y += Move;
        Move += Gravity;
    }

    public double X { get; set; }
    public double Y { get; set; }
    public double Move { get; set; }
    //TEST_ME
    public double Speed { get; set; } = 3;
    //TEST_ME
    public double Gravity { get; set; } = 0;
    public FloatRectangle Hitbox { get; set; }
    public bool IsOnBlock { get; set; }
}
class Level
{
    public Level(GameObject[] objects)
    {
        Objects = objects;
        CollisionList = new BitArray(objects.Length);
    }
    public void Activate(Player player)
    {
        var i = 0;
        foreach (var gameObject in Objects)
        {
            var q = (gameObject.Rectangle & player.Hitbox);
            var isColliding = q.X1 != 0 || q.X2 != 0 || q.Y1 != 0 || q.Y2 != 0;
            if (isColliding)
            {
                switch (gameObject.Type)
                {
                    case GameObjectType.None:
                        break;
                    case GameObjectType.Block:
                        if (!CollisionList[i])
                            player.IsOnBlock = true;
                        break;
                    case GameObjectType.Danger:
                        player.Kill();
                        break;
                    default:
                        if (Test.IsTest)
                        {
                            throw new InvalidDataException("invalid object type");
                        }
                        break;
                }
            }
            else
            {
                if(gameObject.Type == GameObjectType.Block && CollisionList[i])
                {
                    player.IsOnBlock = false;
                }
            }
            CollisionList[i] = isColliding;
            i++;
        }

    }
    public GameObject[] Objects { get; }
    public BitArray CollisionList;
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
    Danger
}