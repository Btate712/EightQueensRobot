using EightQueensRobot.PuzzleSolver;

namespace NQueensSolverTests.PuzzleSolver;

public class NQueensMoveStringValidatorTests
{
    private const int NumberOfRows = 8;
    private readonly NQueensMoveStringValidator _eightQueensMoveValidator = new(NumberOfRows);

    [Fact]
    public void IsValidMove_EmptyString_False()
    {
        // Arrange
        string input = string.Empty;
        
        // Act
        bool result = _eightQueensMoveValidator.IsValidMove(input);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidMove_StringTooShort_False()
    {
        // Arrange
        string input = "123";
        
        // Act
        bool result = _eightQueensMoveValidator.IsValidMove(input);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void IsValidMove_StringTooLong_False()
    {
        // Arrange
        string input = "123456789";
        
        // Act
        bool result = _eightQueensMoveValidator.IsValidMove(input);

        // Assert
        Assert.False(result);
    }    
    
    [Fact]
    public void IsValidMove_AnyNonNumericChar_False()
    {
        // Arrange
        string input = "abcdefgh";
        
        // Act
        bool result = _eightQueensMoveValidator.IsValidMove(input);

        // Assert
        Assert.False(result);
    }   
    
    [Theory]
    [InlineData("12345678")]
    [InlineData("10000000")]
    [InlineData("12340000")]
    public void IsValidMove_ValidMove_True(string input)
    {
        // Act
        bool result = _eightQueensMoveValidator.IsValidMove(input);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("01000000")]
    [InlineData("11001100")]
    public void IsValidMove_HasEmptyRowAboveNonEmptyRow_False(string input)
    {
        // Act
        bool result = _eightQueensMoveValidator.IsValidMove(input);
        
        // Assert
        Assert.False(result);
    }
}