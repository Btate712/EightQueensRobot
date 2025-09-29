namespace EightQueensRobot.IKSolver;

public class DefaultFireflyIterationExitCriteriaHandler(int numberOfIterations) : IFireflyIterationExitCriteriaHandler
{
    private int _iterationCounter = 0;
    
    public bool CanStopIterating()
    {
        _iterationCounter++;
        return _iterationCounter > numberOfIterations;
    }
}