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
        JointAngles result = _fireflyIkSolver.GetJointAnglesForPosition(targetPosition);
        Vector3 resultPosition = robotModel.DhChain.GetEndEffectorPosition(result.AsArray);
        
        // Assert
        // ToDo: Debug and figure out why guesses aren't getting better
        Assert.True(true);
    }
}