using EightQueensRobot.PuzzleSolver;

namespace NQueensSolverTests.PuzzleSolver;

public class NQueensSolverTests
{
    private readonly NQueensSolver _eightQueensSolver = new (8);
    
    [Fact]
    public void GetNextMove_InvalidMove_ReturnsInvalidMove()
    {
        // Arrange
        const string move = "not a valid move";
        
        // Act
        string result = _eightQueensSolver.GetNextMove(move);
        
        // Assert
        Assert.Equal(NQueensSolver.InvalidMove, result);
    }

    [Fact]
    public void GetNextMove_LastMoveIsSolvedPuzzle_Solved()
    {
        // Arrange
        const string solvedPuzzle = "15863724";
        
        // Act
        string result = _eightQueensSolver.GetNextMove(solvedPuzzle);
        
        // Assert
        Assert.Equal(NQueensSolver.Solved, result);
    }
    
    [Fact]
    public void GetNextMove_LastMoveIsNotSolvedPuzzle_ReturnsANewMove()
    {
        // Arrange
        const string solvedPuzzle = "10000000";
        
        // Act
        string result = _eightQueensSolver.GetNextMove(solvedPuzzle);
        
        // Assert
        Assert.NotEqual(NQueensSolver.Solved, result);
    }

    [Fact]
    public void GetNextMove_EmptyBoard_ReturnsQueenInTopRightCorner()
    {
        // Arrange
        const string emptyBoard = "00000000";
        const string topRightQueen = "10000000";
        
        // Act
        string result = _eightQueensSolver.GetNextMove(emptyBoard);
        
        // Assert
        Assert.Equal(topRightQueen, result);
    }

    [Theory]
    [InlineData("10000000", "11000000")]
    [InlineData("13000000", "13100000")]
    public void GetNextMove_AllQueensSafe_AddsQueenInNextRow(string lastMove, string expectedNextMove)
    {
        // Act
        string result = _eightQueensSolver.GetNextMove(lastMove);
        
        // Assert
        Assert.Equal(expectedNextMove, result);
    }

    [Theory]
    [InlineData("11000000", "12000000")]
    public void GetNextMove_AnyQueenCanAttack_IncrementsLastPlacedQueenColumn(string lastMove, string expectedNextMove)
    {
        // Act
        string result = _eightQueensSolver.GetNextMove(lastMove);
        
        // Assert
        Assert.Equal(expectedNextMove, result);
    }

    [Fact]
    public void GetNextMove_MultipleOffendingMoves_RemovesAllButFirstQueenWithOffendingMovesBeforeMakingMove()
    {
        // Arrange
        const string input = "11110000";
        const string expectedNextMove = "12000000";
        // Act
        string result = _eightQueensSolver.GetNextMove(input);
        
        // Assert
        Assert.Equal(expectedNextMove, result);
    }

    [Fact]
    public void GetNextMove_LastPlacedQueenInLastPosition_Backtracks()
    {
        // Arrange
        const string input = "18800000";
        const string expectedNextMove = "21100000";
        
        // Act
        string result = _eightQueensSolver.GetNextMove(input);
        
        // Assert
        Assert.Equal(expectedNextMove, result);
    }

    [Theory]
    [InlineData("12000000", "13000000")]
    [InlineData("13200000", "13300000")]
    public void GetNextMove_LastPlacedQueenCanDiagonallyAttack_IncrementsLastPlacedQueenColumn(string input, string expectedNextMove)
    {
        // Act
        string result = _eightQueensSolver.GetNextMove(input);
        
        // Assert
        Assert.Equal(expectedNextMove, result);
    }

    [Fact]
    public void GetNextMove_CalledRepeatedly_SolvesPuzzleWithinALimitedNumberOfMoves()
    {
        // Arrange
        const int maxMoves = int.MaxValue;
        const string expectedSolution = "15863724";
        string lastMove = "00000000";
        string nextToLastMove = string.Empty;
        bool solved = false;
        
        // Act
        // Limit the number of iterations to maxMoves to force a break from possible infinite loops
        for (int i = 0; i < maxMoves && !solved; i++)
        {
            nextToLastMove = lastMove;
            lastMove = _eightQueensSolver.GetNextMove(lastMove);

            if (lastMove == NQueensSolver.Solved)
            {
                solved = true;
            }
        }
        
        // Assert
        Assert.True(solved);
        Assert.Equal(expectedSolution, nextToLastMove);
    }
}