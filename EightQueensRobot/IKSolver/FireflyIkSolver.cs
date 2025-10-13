using System.Numerics;
using EightQueensRobot.RobotModel;
using EightQueensRobot.Utilities;

namespace EightQueensRobot.IKSolver;

public class FireflyIkSolver(
    IFireflyIterationExitCriteriaHandler exitCriteriaHandler,
    IRobotModel robotModel,
    IFireflySwarmHandler<JointAngles, Vector3> swarmHandler)
    : IIkSolver<JointAngles>
{
    public JointAngles GetJointAnglesForPosition(Vector3 position)
    {
        if (Vector3.Distance(position, Vector3.Zero) > robotModel.MaxReach)
        {
            throw new Exception($"Position {position} exceeds robot model max reach.");
        }
        
        exitCriteriaHandler.Reset();
        Firefly<JointAngles, Vector3>[] swarm = swarmHandler.GenerateFireflySwarm();
        swarmHandler.ProcessSwarm(swarm, position);
        while (!exitCriteriaHandler.CanStopIterating(position))
        {
            swarmHandler.ConcentrateSwarm();
        }
        Firefly<JointAngles, Vector3> bestChoice = swarmHandler.GetClosestFirefly();
        return bestChoice.Data;
    }
}