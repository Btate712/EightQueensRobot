namespace EightQueensRobot.Reporting;

public record QueenPosition(int X, int Y)
{
    public override string ToString()
    {
        if (this == Hopper)
        {
            return "Hopper";
        }
        
        return $"{X},{Y}";
    }

    public static readonly QueenPosition Hopper = new QueenPosition(-1, -1);
}

