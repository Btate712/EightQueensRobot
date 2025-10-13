using EightQueensRobot.IKSolver;
using EightQueensRobot.Utilities;

namespace NQueensSolverTests.FireflyAlgorithmTestSolver;

public class EquationSolverFactory 
{
    public FireflyGenericSolver GetEquationSolver()
    {
        const int numberOfIterations = 1000;
        DefaultFireflyIterationExitCriteriaHandler exitCriteriaHandler = new(numberOfIterations);
        RandomNumberGenerator randomNumberGenerator = new();
        EquationSolverFireflyAttractionHeuristic heuristic = new(randomNumberGenerator);
        EquationSolverSwarmHandler swarmHandler = new(heuristic, randomNumberGenerator);
        return new FireflyGenericSolver(exitCriteriaHandler, swarmHandler);
    }
}