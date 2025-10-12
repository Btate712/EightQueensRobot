namespace EightQueensRobot.Utilities;

public class RandomNumberGenerator : IRandomNumberGenerator
{
    private readonly Random _random = new();
    
    public float GetRandomNumberBetween(float min, float max)
    {
        float range = max - min;
        return _random.NextSingle() * range + min;
    }
}