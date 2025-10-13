using System.Numerics;
using EightQueensRobot.RobotModel;

namespace EightQueensRobot.IKSolver;

public class WithinPositionToleranceExitCriteriaHandler(IFireflySwarmHandler<JointAngles, Vector3> swarmHandler, float tolerance, int maxIterations) : IFireflyIterationExitCriteriaHandler
{
    private int _iterationCount = 0;
    
    public bool CanStopIterating(Vector3 targetPosition)
    {
        _iterationCount++;
        Firefly<JointAngles, Vector3> closestFirefly = swarmHandler.GetClosestFirefly();
        float distance = Vector3.Distance(closestFirefly.Output, targetPosition);
        return distance <= tolerance || _iterationCount > maxIterations;;
    }
    
    public void Reset()
    {
        _iterationCount = 0;
    }
}