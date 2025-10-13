using System.Numerics;
using EightQueensRobot.FKSolver;
using EightQueensRobot.IKSolver;
using EightQueensRobot.Utilities;

namespace NQueensSolverTests.FireflyAlgorithmTestSolver;

public class EquationSolverSwarmHandler(IFireflyAttractionHeuristic<float[], Vector3> heuristic, RandomNumberGenerator randomNumberGenerator)
    : IFireflySwarmHandler<float[], Vector3>
{
    private const int DefaultSwarmSize = 30;
    private float _shortestDistance = Single.MaxValue;
    private Firefly<float[], Vector3>? _closestFirefly;
    private Vector3 _targetPosition;
    private Firefly<float[], Vector3>[] _swarm = [];

    public Firefly<float[], Vector3>[] GenerateFireflySwarm()
    {
        List<Firefly<float[], Vector3>> swarm = [];
        
        for (int i = 0; i < GetSwarmSize(); i++)
        {
            swarm.Add(GenerateFirefly());    
        }
        
        return swarm.ToArray();    }

    public void ProcessSwarm(Firefly<float[], Vector3>[] inputSwarm, Vector3 targetPosition)
    {
        _swarm = inputSwarm;
        _targetPosition = targetPosition;
        _shortestDistance = Single.MaxValue;
        _closestFirefly = inputSwarm[0];
        foreach (Firefly<float[], Vector3> firefly in _swarm)
        {
            ProcessFirefly(firefly);
        }
    }

    private void ProcessFirefly(Firefly<float[], Vector3> firefly)
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
    
    public void ConcentrateSwarm()
    {
        foreach (Firefly<float[], Vector3> firefly in _swarm)
        {
            foreach (Firefly<float[], Vector3> neighbor in _swarm)
            {
                if (firefly.Brightness < neighbor.Brightness)
                {
                    heuristic.MoveFirefly(firefly, neighbor);
                    ProcessFirefly(firefly);
                }
            }
        }
        Console.WriteLine($"Shortest distance: {_shortestDistance}");
    }

    public Firefly<float[], Vector3> GetClosestFirefly()
    {
        return _closestFirefly ?? throw new Exception("No closest firefly found");
    }

    private Vector3 GetPosition(Firefly<float[], Vector3> firefly)
    {
        return SimpleEquationSolver.Solve(firefly.Data);
    }

    private float GetDistanceSquaredToTarget(Vector3 vector)
    {
        return Vector3.DistanceSquared(vector, _targetPosition);
    }

    public int GetSwarmSize()
    {
        return DefaultSwarmSize;
    }
    
    private Firefly<float[], Vector3> GenerateFirefly()
    {
        List<float> angles = [];
        
        for (int variable = 1; variable <= 6; variable++)
        {
            float min = -100;
            float max = 100;
            float randomJointAngle = randomNumberGenerator.GetRandomNumberBetween(min, max);
            angles.Add(randomJointAngle);
        }
        
        return new Firefly<float[], Vector3>(angles.ToArray());
    }
}