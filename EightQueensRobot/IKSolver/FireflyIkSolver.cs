using System.Numerics;
using EightQueensRobot.RobotModel;
using EightQueensRobot.Utilities;

namespace EightQueensRobot.IKSolver;

public class FireflyIkSolver(
    IFireflyIterationExitCriteriaHandler exitCriteriaHandler,
    ISixDofRobotModel robotModel,
    IRandomNumberGenerator randomNumberGenerator,
    IFireflySwarmHandler<SixDofJointData, Vector3> swarmHandler)
    : IIkSolver<SixDofJointData>
{
    public SixDofJointData GetJointAnglesForPosition(Vector3 position)
    {
        Firefly<SixDofJointData, Vector3>[] swarm = GenerateInitialSwarm();
        swarmHandler.ProcessSwarm(swarm, position);
        while (!exitCriteriaHandler.CanStopIterating())
        {
            swarmHandler.ConcentrateSwarm();
        }
        Firefly<SixDofJointData, Vector3> bestChoice = swarmHandler.GetClosestFirefly();
        return bestChoice.Data;
    }

    private Firefly<SixDofJointData, Vector3>[] GenerateInitialSwarm()
    {
        List<Firefly<SixDofJointData, Vector3>> swarm = [];
        
        for (int i = 0; i < swarmHandler.GetSwarmSize(); i++)
        {
            swarm.Add(GenerateFirefly(robotModel));    
        }
        
        return swarm.ToArray();
    }

    private Firefly<SixDofJointData, Vector3> GenerateFirefly(IRobotAngleConstraints constraints)
    {
        float joint1 = randomNumberGenerator.GetRandomNumberBetween(constraints.Axis1MinAngle, constraints.Axis1MaxAngle);
        float joint2 = randomNumberGenerator.GetRandomNumberBetween(constraints.Axis2MinAngle, constraints.Axis2MaxAngle);
        float joint3 = randomNumberGenerator.GetRandomNumberBetween(constraints.Axis3MinAngle, constraints.Axis3MaxAngle);
        float joint4 = randomNumberGenerator.GetRandomNumberBetween(constraints.Axis4MinAngle, constraints.Axis4MaxAngle);
        float joint5 = randomNumberGenerator.GetRandomNumberBetween(constraints.Axis5MinAngle, constraints.Axis5MaxAngle);
        float joint6 = randomNumberGenerator.GetRandomNumberBetween(constraints.Axis6MinAngle, constraints.Axis6MaxAngle);
        
        SixDofJointData jointData = new(joint1, joint2, joint3, joint4, joint5, joint6);
        
        return new Firefly<SixDofJointData, Vector3>(jointData);
    }
}