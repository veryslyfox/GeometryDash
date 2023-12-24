using System;
using System.Windows;
class Player
{
    public Player(Options options)
    {
        Options = options;
    }
    void Tick()
    {
        var isOnBlock = false;
        void ComputeRotate()
        {
            Rotate = MathF.Atan2(Move, 1);
        }
        ComputeRotate();
        PrevPosition = Position;
        Position.X += Velocity;
        Position.Y += Move * Velocity;
    }
    void Click()
    {
        switch (Mode)
        {
            case PlayerMode.Cube:
                Move = Options.JumpY;
                break;
        }
    }
    void Hold()
    {
        switch (Mode)
        {
            case PlayerMode.Spaceship:
                Move += Options.EnginePower;
                break;
        }
    }
    float G(float value) => value * Gravity;
    public Point? PrevPosition;
    public Point Position;
    public float Move { get; set; }
    public PlayerMode Mode { get; set; }
    public Options Options { get; private set; }
    public float Rotate;
    public float Velocity;
    public float Gravity;
}
enum PlayerMode
{
    Cube,
    Spaceship,
}
class Options
{
    public Options(
    float jumpY,
    float enginePower,
    float ufoEnginePower,
    float ufoRotate,
    int playerPixelSize)
    {
        JumpY = jumpY;
        EnginePower = enginePower;
        UfoEnginePower = ufoEnginePower;
        UfoRotate = ufoRotate;
        PlayerPixelSize = playerPixelSize;
    }

    public float JumpY { get; set; }
    public float EnginePower { get; set; }
    public float UfoEnginePower { get; }
    public float UfoRotate { get; internal set; }
    public int PlayerPixelSize { get; }
}
enum Portal
{
    ToCube,
    ToSpaceship,
    GravityToInv,
    GravityToNormal,
}