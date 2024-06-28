using System.Collections.Generic;
class Tree
{
    public Tree Create(Level level)
    {
        foreach (var obj in level.Objects)
        {
            obj.Rectangle
        }
    }
    Tree(Level level, IEnumerable<int> indexes, FloatRectangle boundsLeft, FloatRectangle boundsRight)
    {
        var cuttedLeft = CutVertical(boundsLeft);
        var cuttedRight = CutVertical(boundsRight);
        Level = level;
        if (boundsLeft.GetArea() < 50000)
        {
            return;
        }
        Left = new(level, CutLevel(boundsLeft), cuttedLeft.Item1, cuttedLeft.Item2);
        Right = new(level, CutLevel(boundsRight), cuttedRight.Item1, cuttedRight.Item2);
        Indexes = new(indexes);
    }
    List<int> CutLevel(FloatRectangle bounds)
    {
        var result = new List<int>();
        var i = 0;
        foreach (var obj in Level.Objects)
        {
            if ((obj.Rectangle & bounds) == obj.Rectangle)
            {
                result.Add(i);
            }
            i++;
        }
        return result;
    }
    static (FloatRectangle, FloatRectangle) CutVertical(FloatRectangle input)
    {
        return (new(input.X1, input.Y1, (input.X1 + input.X2) / 2, input.Y2), new((input.X1 + input.X2) / 2, input.Y1, input.X2, input.Y2));
    }
    public Level Level { get; }
    public Tree? Left { get; }
    public Tree? Right { get; }
    public HashSet<int>? Indexes { get; }
}