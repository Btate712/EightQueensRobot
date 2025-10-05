using System.Numerics;
using EightQueensRobot.RobotModel;

namespace EightQueensRobot.IKSolver;

public class DefaultFireflySwarmHandler(IRobotModel robotModel, IFireflyAttractionHeuristic<JointAngles, Vector3> heuristic)
    : IFireflySwarmHandler<JointAngles, Vector3>
{
    private const int DefaultSwarmSize = 30;
    private float _shortestDistance = Single.MaxValue;
    private Firefly<JointAngles, Vector3>? _closestFirefly;
    private Vector3 _targetPosition;
    private Firefly<JointAngles, Vector3>[] _swarm = [];

    public void ProcessSwarm(Firefly<JointAngles, Vector3>[] inputSwarm, Vector3 targetPosition)
    {
        _swarm = inputSwarm;
        _targetPosition = targetPosition;
        _shortestDistance = Single.MaxValue;
        _closestFirefly = inputSwarm[0];
        foreach (Firefly<JointAngles, Vector3> firefly in _swarm)
        {
            Vector3 position = GetPosition(firefly);
            float distance = GetDistanceSquaredToTarget(position);
            firefly.Output = position;
            firefly.Fitness = distance;
            if (distance < _shortestDistance)
            {
                _shortestDistance = distance;
                _closestFirefly = firefly;
            }
        }
    }

    public void ConcentrateSwarm()
    {
        Firefly<JointAngles, Vector3>[] newSwarm = _swarm.Select(ff => ff.Clone()).ToArray();
        foreach (Firefly<JointAngles, Vector3> firefly in newSwarm)
        {
            foreach (Firefly<JointAngles, Vector3> neighbor in newSwarm)
            {
                if (firefly.Fitness > neighbor.Fitness)
                {
                    heuristic.MoveFirefly(firefly, neighbor);
                }
            }
        }
        
        ProcessSwarm(newSwarm, _targetPosition);
    }

    public Firefly<JointAngles, Vector3> GetClosestFirefly()
    {
        return _closestFirefly ?? throw new Exception("No closest firefly found");
    }

    private Vector3 GetPosition(Firefly<JointAngles, Vector3> firefly)
    {
        return robotModel.DhChain.GetEndEffectorPosition(firefly.Data.AsArray);
    }

    private float GetDistanceSquaredToTarget(Vector3 vector)
    {
        return Vector3.DistanceSquared(vector, _targetPosition);
    }

    public int GetSwarmSize()
    {
        return DefaultSwarmSize;
    }
}
