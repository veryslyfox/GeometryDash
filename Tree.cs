using System.Collections.Generic;
class Tree
{
    public static Tree Create(Level level)
    {
        int[] Sequence(int length)
        {
            var result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = i;
            }
            return result;
        }
        var acc = new RectAccumulator();
        foreach (var obj in level.Objects)
        {
            acc.Add(obj.Rectangle);            
        }
        var bounds = CutVertical(acc.Result);
        return new(level, Sequence(level.Objects.Length), bounds.Item1, bounds.Item2);
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