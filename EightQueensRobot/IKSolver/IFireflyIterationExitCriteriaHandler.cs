using System.Numerics;

namespace EightQueensRobot.IKSolver;

public interface IFireflyIterationExitCriteriaHandler
{
    bool CanStopIterating(Vector3 firefly);
    void Reset();
}