using System.Numerics;
using BenchmarkDotNet.Attributes;
using EightQueensRobot.IKSolver;
using EightQueensRobot.RobotModel;
using EightQueensRobot.Utilities;

namespace Benchmarker.Benchmarks;

public class IkSolverRunner
{
    private const int TargetPositionCount = 100;
    private readonly FireflyIkSolverFactory _factory = new();
    private readonly IIkSolver<JointAngles> _defaultSolver;
    private readonly IIkSolver<JointAngles> _withinPositionToleranceSolver;
    private readonly IIkSolver<JointAngles> _defaultWithCachingSolver;
    private readonly IIkSolver<JointAngles> _withinPositionToleranceWithCachingSolver;
    private readonly IIkSolver<JointAngles> _swarmSizeOptimizedSolver;
    private readonly Vector3[] _targetPositions = new Vector3[100];
    private readonly RandomNumberGenerator _randomNumberGenerator = new();
    
    public IkSolverRunner()
    {
        _defaultSolver = _factory.GetDefaultIkSolver();
        _withinPositionToleranceSolver = _factory.GetWithinToleranceSolver();
        _defaultWithCachingSolver = _factory.GetDefaultIkSolverWithCache();
        _withinPositionToleranceWithCachingSolver = _factory.GetWithinToleranceSolverWithCache();
        _swarmSizeOptimizedSolver = _factory.GetSwarmSizeOptimizedSolverWithCache();
        const float maxReach = 1.0f;
        
        for (int i = 0; i < TargetPositionCount; i++)
        {
            bool validValueFound = false;

            while (!validValueFound)
            {
                Vector3 targetPosition = GetTargetPosition();
                if (Vector3.Distance(targetPosition, Vector3.Zero) <= maxReach)
                {
                    validValueFound = true;
                    _targetPositions[i] = targetPosition;
                }
            }
        }
    }

    private Vector3 GetTargetPosition()
    {
        float x = _randomNumberGenerator.GetRandomNumberBetween(-1, 1);
        float y = _randomNumberGenerator.GetRandomNumberBetween(-1, 1);
        float z = _randomNumberGenerator.GetRandomNumberBetween(-1, 1);
        return new Vector3(x, y, z);
    }
    
    [Benchmark]
    public void RunDefault()
    {
        JointAngles[] unused = _targetPositions.Select(targetPosition => _defaultSolver.GetJointAnglesForPosition(targetPosition)).ToArray();
    }

    [Benchmark]
    public void RunWithinPositionTolerance()
    {
        JointAngles[] unused = _targetPositions.Select(targetPosition => _withinPositionToleranceSolver.GetJointAnglesForPosition(targetPosition)).ToArray();
    }
    
    [Benchmark]
    public void RunDefaultWithCaching()
    {
        JointAngles[] unused = _targetPositions.Select(targetPosition => _defaultWithCachingSolver.GetJointAnglesForPosition(targetPosition)).ToArray();
    }  
    
    [Benchmark]
    public void RunWithinPositionToleranceWithCaching()
    {
        JointAngles[] unused = _targetPositions.Select(targetPosition => _withinPositionToleranceWithCachingSolver.GetJointAnglesForPosition(targetPosition)).ToArray();
    }

    [Benchmark]
    public void RunSwarmSizeOptimizedWithinPositionToleranceWithCaching()
    {
        JointAngles[] unused = _targetPositions.Select(targetPosition => _swarmSizeOptimizedSolver.GetJointAnglesForPosition(targetPosition)).ToArray();
    }
}