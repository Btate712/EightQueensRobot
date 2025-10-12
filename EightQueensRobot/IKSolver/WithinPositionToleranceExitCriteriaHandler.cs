using System.Numerics;
using EightQueensRobot.RobotModel;

namespace EightQueensRobot.IKSolver;

public class WithinPositionToleranceExitCriteriaHandler(IFireflySwarmHandler<JointAngles, Vector3> swarmHandler, float tolerance) : IFireflyIterationExitCriteriaHandler
{
    public bool CanStopIterating(Vector3 targetPosition)
    {
        Firefly<JointAngles, Vector3> closestFirefly = swarmHandler.GetClosestFirefly();
        float distance = Vector3.Distance(closestFirefly.Output, targetPosition);
        return distance <= tolerance;
    }
    
    public void Reset()
    {
    }
}