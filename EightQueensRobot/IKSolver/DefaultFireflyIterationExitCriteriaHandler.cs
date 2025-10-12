using System.Numerics;

namespace EightQueensRobot.IKSolver;

public class DefaultFireflyIterationExitCriteriaHandler(int numberOfIterations) : IFireflyIterationExitCriteriaHandler
{
    private int _iterationCounter;
    
    public bool CanStopIterating(Vector3 targetPosition)
    {
        _iterationCounter++;
        return _iterationCounter > numberOfIterations;
    }

    public void Reset()
    {
        _iterationCounter = 0;
    }
}