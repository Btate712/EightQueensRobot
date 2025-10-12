namespace EightQueensRobot.Utilities;

public static class AngleUnitConverter
{
    public static float ToRadians(this float degrees) => (float)(degrees * Math.PI / 180);
    public static float ToDegrees(this float radians) => (float)(radians * 180 / Math.PI);
}