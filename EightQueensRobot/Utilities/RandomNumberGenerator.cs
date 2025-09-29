namespace EightQueensRobot.Utilities;

public class RandomNumberGenerator
{
    public RandomNumberGenerator()
    {
        _random = new Random();
    }
    
    private readonly Random _random;
    
    public float GetRandomNumberBetween(float min, float max)
    {
        float range = max - min;
        return _random.NextSingle() * range + min;
    }
}