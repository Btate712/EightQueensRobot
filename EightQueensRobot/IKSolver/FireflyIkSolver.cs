using System.Numerics;
using EightQueensRobot.RobotModel;
using EightQueensRobot.Utilities;

namespace EightQueensRobot.IKSolver;

public class FireflyIkSolver(
    IFireflyIterationExitCriteriaHandler exitCriteriaHandler,
    IRobotModel robotModel,
    IRandomNumberGenerator randomNumberGenerator,
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
        Firefly<JointAngles, Vector3>[] swarm = GenerateInitialSwarm();
        swarmHandler.ProcessSwarm(swarm, position);
        while (!exitCriteriaHandler.CanStopIterating(position))
        {
            swarmHandler.ConcentrateSwarm();
        }
        Firefly<JointAngles, Vector3> bestChoice = swarmHandler.GetClosestFirefly();
        return bestChoice.Data;
    }

    private Firefly<JointAngles, Vector3>[] GenerateInitialSwarm()
    {
        List<Firefly<JointAngles, Vector3>> swarm = [];
        
        for (int i = 0; i < swarmHandler.GetSwarmSize(); i++)
        {
            swarm.Add(GenerateFirefly(robotModel));    
        }
        
        return swarm.ToArray();
    }

    private Firefly<JointAngles, Vector3> GenerateFirefly(IRobotModel model)
    {
        List<double> angles = [];
        
        for (int joint = 1; joint <= model.GetDoF(); joint++)
        {
            float min = model.GetMinAngle(joint);
            float max = model.GetMaxAngle(joint);
            float randomJointAngle = randomNumberGenerator.GetRandomNumberBetween(min, max);
            angles.Add(randomJointAngle);
        }
        
        JointAngles jointAngles = new(angles.ToArray());
        
        return new Firefly<JointAngles, Vector3>(jointAngles);
    }
}