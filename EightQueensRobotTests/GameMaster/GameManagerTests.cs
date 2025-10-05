using EightQueensRobot.GameMaster;

namespace NQueensSolverTests.GameMaster;

public class GameManagerTests
{
    [Fact]
    public void Run_ExecutesTheGame()
    {
        // Arrange
        GameFactory factory = new();
        GameManager gameManager = factory.GetDefaultGame();
        
        // Act
        gameManager.Run();
        
        // Assert
        Assert.True(true);
    }
}