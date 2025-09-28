using EightQueensRobot.Utilities;

namespace NQueensSolverTests.Utilities;

public class CharacterExtensionsTests
{
    [Theory]
    [InlineData('0', 0)]
    [InlineData('1', 1)]
    [InlineData('2', 2)]
    [InlineData('3', 3)]
    [InlineData('4', 4)]
    [InlineData('5', 5)]
    [InlineData('6', 6)]
    [InlineData('7', 7)]
    [InlineData('8', 8)]
    [InlineData('9', 9)]
    public void AsInt_IsDigit_ReturnsDigitAsInt(char input, int value)
    {
        // Act
        int result = input.AsInt();
        
        // Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData('A')]
    [InlineData('-')]
    [InlineData('a')]
    [InlineData('@')]
    [InlineData(' ')]
    [InlineData('\n')]
    public void AsInt_IsNotDigit_ReturnsNegativeOne(char input)
    {
        // Act
        int result = input.AsInt();
        
        // Assert
        Assert.Equal(-1, result);
    }
}