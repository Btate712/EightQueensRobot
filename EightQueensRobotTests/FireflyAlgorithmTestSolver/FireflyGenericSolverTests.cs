using System.Numerics;
using EightQueensRobot.FKSolver;

namespace NQueensSolverTests.FireflyAlgorithmTestSolver;

public class FireflyGenericSolverTests
{
    private readonly FireflyGenericSolver _solver;
    public FireflyGenericSolverTests()
    {
        EquationSolverFactory factory = new();
        _solver = factory.GetEquationSolver();
    }
    
    [Fact]
    public void GetInputsForOutput_SolvesEquations()
    {
        // Arrange
        const float targetX = 10;
        const float targetY = 20;
        const float targetZ = 30;
        
        // Act
        float[] inputs = _solver.GetInputsForOutput(new Vector3(targetX, targetY, targetZ));
        Vector3 resultBasedOnSolvedInputs = SimpleEquationSolver.Solve(inputs);

        float xDistance = Math.Abs(targetX - resultBasedOnSolvedInputs.X);
        float yDistance = Math.Abs(targetY - resultBasedOnSolvedInputs.Y);
        float zDistance = Math.Abs(targetZ - resultBasedOnSolvedInputs.Z);
        float distanceSum = xDistance + yDistance + zDistance;
        
        Assert.Equal(targetX, resultBasedOnSolvedInputs.X, 1);
        Assert.Equal(targetY, resultBasedOnSolvedInputs.Y, 1);
        Assert.Equal(targetZ, resultBasedOnSolvedInputs.Z, 1);
    }
}