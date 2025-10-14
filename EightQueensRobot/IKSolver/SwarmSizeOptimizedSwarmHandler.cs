using System.Numerics;
using EightQueensRobot.RobotModel;
using EightQueensRobot.Utilities;

namespace EightQueensRobot.IKSolver;

public class SwarmSizeOptimizedSwarmHandler(
    IRobotModel robotModel, 
    IFireflyAttractionHeuristic<JointAngles, Vector3> heuristic, 
    RandomNumberGenerator randomNumberGenerator,
    IFireflyCache<JointAngles, Vector3> fireflyCache)
    : IFireflySwarmHandler<JointAngles, Vector3>
{
    private const int DefaultSwarmSize = 30;
    private const int ReducedSwarmSize = 10;
    private float _shortestDistance = Single.MaxValue;
    private Firefly<JointAngles, Vector3>? _closestFirefly;
    private Vector3 _targetPosition;
    private Firefly<JointAngles, Vector3>[] _swarm = [];
    
    public Firefly<JointAngles, Vector3> GetClosestFirefly()
    {
        return _closestFirefly ?? throw new Exception("No closest firefly found");
    }

    public Firefly<JointAngles, Vector3>[] GenerateFireflySwarm()
    {
        List<Firefly<JointAngles, Vector3>> swarm = [];
        
        for (int i = 0; i < GetSwarmSize(); i++)
        {
            swarm.Add(GenerateFirefly(robotModel));    
        }
        
        return swarm.ToArray();    
    }
    
    private Firefly<JointAngles, Vector3> GenerateFirefly(IRobotModel model)
    {
        JointAngleBoundaries cachedLimits = fireflyCache.GetCachedBoundaries(_targetPosition);

        List<double> angles = [];
        
        for (int joint = 1; joint <= model.GetDoF(); joint++)
        {
            float min = GetMinAngle(joint, model, cachedLimits);
            float max = GetMaxAngle(joint, model, cachedLimits);
            float randomJointAngle = randomNumberGenerator.GetRandomNumberBetween(min, max);
            angles.Add(randomJointAngle);
        }
        
        JointAngles jointAngles = new(angles.ToArray());
        
        return new Firefly<JointAngles, Vector3>(jointAngles);
    }

    private float GetMinAngle(int jointNumber, IRobotModel model, JointAngleBoundaries cachedLimits)
    {
        float minLimit = cachedLimits == JointAngleBoundaries.Null
            ? float.MinValue
            : cachedLimits.GetMinValueForJoint(jointNumber);

        float modelMin = model.GetMinAngle(jointNumber);
        
        return Math.Max(minLimit, modelMin);
    }
    
    private float GetMaxAngle(int jointNumber, IRobotModel model, JointAngleBoundaries cachedLimits)
    {
        float maxLimit = cachedLimits == JointAngleBoundaries.Null
            ? float.MaxValue
            : cachedLimits.GetMaxValueForJoint(jointNumber);

        float modelMin = model.GetMaxAngle(jointNumber);
        
        return Math.Min(maxLimit, modelMin);
    }
    
    public void ProcessSwarm(Firefly<JointAngles, Vector3>[] inputSwarm, Vector3 targetPosition)
    {
        _swarm = inputSwarm;
        _targetPosition = targetPosition;
        _shortestDistance = Single.MaxValue;
        _closestFirefly = inputSwarm[0];
        foreach (Firefly<JointAngles, Vector3> firefly in _swarm)
        {
            ProcessFirefly(firefly);
        }
        fireflyCache.CacheSwarm(_swarm);
    }

    private void ProcessFirefly(Firefly<JointAngles, Vector3> firefly)
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
    
    private Vector3 GetPosition(Firefly<JointAngles, Vector3> firefly)
    {
        return robotModel.DhChain.GetEndEffectorPosition(firefly.Data.AsArray);
    }

    private float GetDistanceSquaredToTarget(Vector3 vector)
    {
        return Vector3.DistanceSquared(vector, _targetPosition);
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
                    ProcessFirefly(firefly);
                }
            }
        }
        
        ProcessSwarm(newSwarm, _targetPosition);    }

    public int GetSwarmSize()
    {
        return fireflyCache.HasCachedValue(_targetPosition) ? ReducedSwarmSize : DefaultSwarmSize;
    }
}