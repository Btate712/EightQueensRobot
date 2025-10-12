using EightQueensRobot.Utilities;

namespace NQueensSolverTests.Utilities;

public class FloatExtensionTests
{
    [Fact]
    public void ToRadians_PositiveInput_PositiveOutput()
    {
        // Arrange
        const float input = 180f;
        const float expected = 3.14159f;
        
        // Act
        float actual = input.ToRadians();
        
        // Assert
        Assert.Equal(expected, actual, 0.001);
    }
}