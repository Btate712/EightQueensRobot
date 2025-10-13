using System.Numerics;
using EightQueensRobot.RobotModel;
using EightQueensRobot.Utilities;

namespace EightQueensRobot.IKSolver;

public class FireflyIkSolverFactory : IIkSolverFactory<JointAngles>
{
    public IIkSolver<JointAngles> GetDefaultIkSolver()
    {
        const int numberOfIterations = 1000;
        DefaultFireflyIterationExitCriteriaHandler exitCriteriaHandler = new(numberOfIterations);
        Sungur370 robotModel = new();
        RandomNumberGenerator randomNumberGenerator = new();
        NullFireflyCache<JointAngles, Vector3> nullFireflyCache = new();
        DefaultFireflyAttractionHeuristic heuristic = new(randomNumberGenerator, robotModel);
        DefaultFireflySwarmHandler swarmHandler = new(robotModel, heuristic, randomNumberGenerator, nullFireflyCache);

        return new FireflyIkSolver(
            exitCriteriaHandler: exitCriteriaHandler,
            robotModel: robotModel,
            swarmHandler: swarmHandler
            );
    }

    public IIkSolver<JointAngles> GetWithinToleranceSolver()
    {
        const int maxIterations = 1000;
        const float tolerance = 0.001f;
        Sungur370 robotModel = new();
        RandomNumberGenerator randomNumberGenerator = new();
        DefaultFireflyAttractionHeuristic heuristic = new(randomNumberGenerator, robotModel);
        NullFireflyCache<JointAngles, Vector3> nullFireflyCache = new();
        DefaultFireflySwarmHandler swarmHandler = new(robotModel, heuristic, randomNumberGenerator, nullFireflyCache);
        WithinPositionToleranceExitCriteriaHandler exitCriteriaHandler = new(swarmHandler, tolerance, maxIterations);

        return new FireflyIkSolver(
            exitCriteriaHandler: exitCriteriaHandler,
            robotModel: robotModel,
            swarmHandler: swarmHandler
            );
    }
}