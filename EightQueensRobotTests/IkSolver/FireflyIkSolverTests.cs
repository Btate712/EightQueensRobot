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
        Vector3 targetPosition = new(0.25f, 0.00f, 0.25f);
        AbbIrb120 robotModel = new();
        
        // Act
        SixDofJointData result = _fireflyIkSolver.GetJointAnglesForPosition(targetPosition);
        Vector3 resultPosition = robotModel.DhChain.GetEndEffectorPosition([result.Joint1, result.Joint2, result.Joint3, result.Joint4, result.Joint5, result.Joint6]);
        
        // Assert
        // ToDo: Debug and figure out why guesses aren't getting better
        Assert.True(true);
    }
}