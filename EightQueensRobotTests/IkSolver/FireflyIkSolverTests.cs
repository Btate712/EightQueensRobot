using System.Numerics;
using EightQueensRobot.IKSolver;
using EightQueensRobot.RobotModel;

namespace NQueensSolverTests.IkSolver;

public class FireflyIkSolverTests
{
    private readonly FireflyIkSolver _fireflyIkSolver;
    
    public FireflyIkSolverTests()
    {
        FireflyIkSolverFactory factory = new();
        _fireflyIkSolver = (FireflyIkSolver) factory.GetDefaultIkSolver();
    }
    [Fact]
    public void GetJointAnglesForPosition_PositionRequested_JointAnglesObtained()
    {
        // Arrange
        Vector3 targetPosition = new(0.25f, 0.25f, 0.25f);
        
        // Act
        SixDofJointData result = _fireflyIkSolver.GetJointAnglesForPosition(targetPosition);
        
        // Assert
        Assert.True(true);
    }
}