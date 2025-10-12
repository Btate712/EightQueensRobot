using System.Numerics;
using EightQueensRobot.IKSolver;
using EightQueensRobot.RobotModel;

namespace NQueensSolverTests.IkSolver;

public class FireflyIkSolverTests
{
    private readonly FireflyIkSolver _defaultIkSolver;
    private readonly FireflyIkSolver _withinToleranceIkSolver;
    
    public FireflyIkSolverTests()
    {
        FireflyIkSolverFactory factory = new();
        _defaultIkSolver = (FireflyIkSolver) factory.GetDefaultIkSolver();
        _withinToleranceIkSolver = (FireflyIkSolver)factory.GetWithinToleranceSolver();
    }
    
    [Theory]
    [InlineData(-0.247487f, 1.009619f, 0.50f)]
    [InlineData(0.07861869, -0.14541759, 0.050121572)]
    [InlineData(-0.25, -0.25, -0.15)]
    public void GetJointAnglesForPosition_DefaultSolver_JointAnglesObtained(float positionX, float positionY, float positionZ)
    {
        // Arrange
        Vector3 targetPosition = new(positionX, positionY, positionZ);
        Sungur370 robotModel = new();
        const float tolerance = 0.00005f;
        
        // Act
        JointAngles result = _defaultIkSolver.GetJointAnglesForPosition(targetPosition);
        Vector3 resultPosition = robotModel.DhChain.GetEndEffectorPosition(result.AsArray);
        
        // Assert
        Assert.Equal(targetPosition.X, resultPosition.X, tolerance);
        Assert.Equal(targetPosition.Y, resultPosition.Y, tolerance);
        Assert.Equal(targetPosition.Z, resultPosition.Z, tolerance);
    }   
    
    [Theory]
    [InlineData(-0.247487f, 1.009619f, 0.50f)]
    [InlineData(0.07861869, -0.14541759, 0.050121572)]
    [InlineData(-0.25, -0.25, -0.15)]
    public void GetJointAnglesForPosition_WithinToleranceSolver_JointAnglesObtained(float positionX, float positionY, float positionZ)
    {
        // Arrange
        Vector3 targetPosition = new(positionX, positionY, positionZ);
        Sungur370 robotModel = new();
        const float tolerance = 0.001f;
        
        // Act
        JointAngles result = _withinToleranceIkSolver.GetJointAnglesForPosition(targetPosition);
        Vector3 resultPosition = robotModel.DhChain.GetEndEffectorPosition(result.AsArray);
        
        // Assert
        Assert.Equal(targetPosition.X, resultPosition.X, tolerance);
        Assert.Equal(targetPosition.Y, resultPosition.Y, tolerance);
        Assert.Equal(targetPosition.Z, resultPosition.Z, tolerance);
    }  
}