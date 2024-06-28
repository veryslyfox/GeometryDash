using System.Collections;
using System.Diagnostics;
using System.Windows.Input;
class Player
{
    public Player(double x, double y, FloatRectangle hitbox, double move, Level level)
    {
        X = x;
        Y = y;
        SpawnPositionX = x;
        SpawnPositionY = y;
        Hitbox = hitbox;
        Move = move;
        Level = level;
        LastTickTime = Stopwatch.GetTimestamp();
    }

    public void KeyDown(object sender, KeyEventArgs args)
    {
        if (Level.IsSleep)
        {
            return;
        }
        if (args.Key != Key.Up && args.Key != Key.Space)
        {
            return;
        }
        Move = -Speed * Angle;
        IsOnBlock = false;
    }
    internal void KeyUp(object sender, KeyEventArgs args)
    {
        if (args.Key != Key.Up && args.Key != Key.Space)
        {
            return;
        }
        Move = Speed * Angle;
    }
    public void Kill()
    {
        Level.RespawnTime = Stopwatch.GetTimestamp() + (RespawnTime * Stopwatch.Frequency) / 1000;
        Level.IsSleep = true;
    }
    public void Respawn()
    {
        Hitbox = Hitbox.Move(SpawnPositionX - X, SpawnPositionY - Y);
        X = SpawnPositionX;
        Y = SpawnPositionY;
    }

    public void Tick()
    {
        var now = Stopwatch.GetTimestamp();
        var delay = (((double)(now - LastTickTime) * 200 / Stopwatch.Frequency));
        LastTickTime = now;
        if (Level.IsSleep)
        {
            return;
        }
        Hitbox = Hitbox.Move(Speed * delay, Move * delay);
        X += Speed * delay;
        if (IsOnBlock)
        {
            Move = 0;
        }
        Y += Move * delay;
        Move += Gravity * delay;
    }

    public double X { get; set; }
    public double Y { get; set; }
    public long LastTickTime;
    public double SpawnPositionX;
    public double SpawnPositionY;
    public Level Level;
    public double Move { get; set; }
    //TEST_ME
    public double Speed { get; set; } = 1;
    //TEST_ME
    public double Gravity { get; set; } = 0;
    //TEST_ME
    public double Angle = 0;
    public int RespawnTime = 1000;
    public FloatRectangle Hitbox { get; set; }
    public bool IsOnBlock { get; set; }
}
class Level
{
    public Level(GameObject[] objects)
    {
        Objects = objects;
        CollisionList = new BitArray(objects.Length);
        RespawnTime = long.MaxValue;
    }
    public void Activate(Player player)
    {
        UpdateSleep(player);
        if (IsSleep)
        {
            return;
        }
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
                if (gameObject.Type == GameObjectType.Block && CollisionList[i])
                {
                    player.IsOnBlock = false;
                }
            }
            CollisionList[i] = isColliding;
            i++;
        }
    }
    public void UpdateSleep(Player player)
    {
        if ((Stopwatch.GetTimestamp() > RespawnTime) & IsSleep)
        {
            player.Respawn();
            IsSleep = false;
        }
    }
    public bool IsSleep;
    public long RespawnTime;
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