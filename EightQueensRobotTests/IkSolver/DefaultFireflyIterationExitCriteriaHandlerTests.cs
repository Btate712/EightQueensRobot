using System.Numerics;
using EightQueensRobot.IKSolver;

namespace NQueensSolverTests.IkSolver;

public class DefaultFireflyIterationExitCriteriaHandlerTests
{
    [Fact]
    public void CanStopIterating_StopsAtAppropriateTime()
    {
        // Arrange
        const int numberOfIterations = 10;
        const int infiniteLoopPreventionLimit = Int32.MaxValue;
        DefaultFireflyIterationExitCriteriaHandler iterationHandler = new(numberOfIterations);
        int counter = 0;
        
        // Act
        while (!iterationHandler.CanStopIterating(new Vector3()) && counter < infiniteLoopPreventionLimit)
        {
            counter++;
        }

        // Assert
        Assert.Equal(numberOfIterations, counter);
    }
}