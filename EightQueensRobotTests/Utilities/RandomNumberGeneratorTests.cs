using EightQueensRobot.Utilities;

namespace NQueensSolverTests.Utilities;

public class RandomNumberGeneratorTests
{
    private readonly RandomNumberGenerator _generator;
    
    public RandomNumberGeneratorTests()
    {
        _generator = new RandomNumberGenerator();    
    }

    [Fact]
    public void GetRandomNumberBetween_GeneratesRandomNumbers()
    {
        // Arrange
        const int runCount = 1_000_000;
        const float min = -5;
        const float max = 5;
        const int tolerance = (int)(0.05f * runCount);
        const int expectedEachQuartile = runCount / 4;
        const int minExpectedEachQuartile = expectedEachQuartile - tolerance;
        const int maxExpectedEachQuartile = expectedEachQuartile + tolerance;
        const float range = max - min;
        const float quartileSize = range / 4;
        const float firstQuartileTop = min + 1 * quartileSize;
        const float secondQuartileTop = min + 2 * quartileSize;
        const float thirdQuartileTop = min + 3 * quartileSize;
        
        int firstQuartileCount = 0;
        int secondQuartileCount = 0;
        int thirdQuartileCount = 0;
        int fourthQuartileCount = 0;
        int outsideRangeCount = 0;
        
        // Act
        for (int i = 0; i < runCount; i++)
        {
            float value = _generator.GetRandomNumberBetween(min, max);
            switch (value)
            {
                case >= min and < firstQuartileTop:
                    firstQuartileCount++;
                    break;
                case >= firstQuartileTop and < secondQuartileTop:
                    secondQuartileCount++;
                    break;
                case >= secondQuartileTop and < thirdQuartileTop:
                    thirdQuartileCount++;
                    break;
                case >= thirdQuartileTop and <= max:
                    fourthQuartileCount++;
                    break;
                default:
                    outsideRangeCount++;
                    break;
            }
        }
        
        // Assert
        Assert.Equal(0, outsideRangeCount);
        Assert.InRange(firstQuartileCount, minExpectedEachQuartile, maxExpectedEachQuartile);
        Assert.InRange(secondQuartileCount, minExpectedEachQuartile, maxExpectedEachQuartile);
        Assert.InRange(thirdQuartileCount, minExpectedEachQuartile, maxExpectedEachQuartile);
        Assert.InRange(fourthQuartileCount, minExpectedEachQuartile, maxExpectedEachQuartile);
    }
}