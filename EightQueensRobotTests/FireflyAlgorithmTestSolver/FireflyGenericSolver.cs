using System.Numerics;
using EightQueensRobot.IKSolver;
using EightQueensRobot.Utilities;

namespace NQueensSolverTests.FireflyAlgorithmTestSolver;

public class FireflyGenericSolver(IFireflyIterationExitCriteriaHandler exitCriteriaHandler,
    IFireflySwarmHandler<float[], Vector3> swarmHandler)
{
    public float[] GetInputsForOutput(Vector3 output)
    {
        int iterationCount = 0;
        Firefly<float[], Vector3>[] swarm = swarmHandler.GenerateFireflySwarm();
        swarmHandler.ProcessSwarm(swarm, output);
        while (!exitCriteriaHandler.CanStopIterating(output))
        {
            iterationCount++;
            Console.WriteLine($"Iteration: {iterationCount}");
            swarmHandler.ConcentrateSwarm();
            
        }
        Firefly<float[], Vector3> bestChoice = swarmHandler.GetClosestFirefly();
        return bestChoice.Data;
    }
}