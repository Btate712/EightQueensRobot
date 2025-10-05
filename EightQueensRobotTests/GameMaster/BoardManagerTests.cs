using System.Numerics;
using EightQueensRobot.GameMaster;

namespace NQueensSolverTests.GameMaster;

public class BoardManagerTests
{
    private const int BoardSize = 8;
    private const float BoardWidth = 80f;
    
    private readonly BoardManager _boardManager;
    private readonly Vector3 _corner1 = new Vector3(0, 0, 0);
    private readonly Vector3 _corner2 = new Vector3(BoardWidth, BoardWidth, 0);
    
    public BoardManagerTests()
    {
        _boardManager = new BoardManager(BoardSize, _corner1, _corner2);
    }
    
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(BoardSize + 1, 0)]
    [InlineData(0, BoardSize + 1)]
    public void GetSquareCenter_InvalidSquare_Throws(int x, int y)
    {
        // Act and Assert
        Assert.Throws<ArgumentException>(() => _boardManager.GetSquareCenter(x, y));
    }

    [Theory]
    [InlineData(1, 1, 5f, 5f)]
    [InlineData(2, 1, 15f, 5f)]
    [InlineData(2, 2, 15f, 15f)]
    [InlineData(8, 8, 75f, 75f)]
    public void GetSquareCenter_ValidSquare_ReturnsCorrectCenter(int boardX, int boardY, float expectedX, float expectedY)
    {
        // Act
        Vector3 actual = _boardManager.GetSquareCenter(boardX, boardY);
        
        // Assert
        Assert.Equal(expectedX, actual.X);
        Assert.Equal(expectedY, actual.Y);
    }
}