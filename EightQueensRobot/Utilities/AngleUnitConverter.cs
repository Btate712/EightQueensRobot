namespace EightQueensRobot.Utilities;

public static class AngleUnitConverter
{
    public static double ToRadians(this float degrees) => degrees * Math.PI / 180;
    public static double ToDegrees(this float radians) => radians * 180 / Math.PI;
}