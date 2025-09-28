namespace EightQueensRobot.Utilities;

public class AngleUnitConverter
{
    public static double ToRadians(float degrees) => degrees * Math.PI / 180;
    public static double ToDegrees(float radians) => radians * 180 / Math.PI;
}