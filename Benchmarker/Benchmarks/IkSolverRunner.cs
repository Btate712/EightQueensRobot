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
    private readonly IIkSolver<JointAngles> _solver;
    private readonly Vector3[] _targetPositions = new Vector3[100];
    
    public IkSolverRunner()
    {
        _solver = _factory.GetDefaultIkSolver();
        RandomNumberGenerator randomNumberGenerator = new();
        for (int i = 0; i < TargetPositionCount; i++)
        {
            float x = randomNumberGenerator.GetRandomNumberBetween(-1, 1);
            float y = randomNumberGenerator.GetRandomNumberBetween(-1, 1);
            float z = randomNumberGenerator.GetRandomNumberBetween(-1, 1);
            _targetPositions[i] = new Vector3(x, y, z);
        }
    }
    
    [Benchmark]
    public void RunDefault()
    {
        JointAngles[] unused = _targetPositions.Select(targetPosition => _solver.GetJointAnglesForPosition(targetPosition)).ToArray();
    }
}