namespace EightQueensRobot.Utilities;

public static class CharacterExtensions
{
    public static int AsInt(this char c)
    {
        if (c is >= '0' and <= '9')
        {
            return c - '0';
        }

        return -1;
    }
}