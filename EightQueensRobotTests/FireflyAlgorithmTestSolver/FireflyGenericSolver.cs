using System.Numerics;
using EightQueensRobot.IKSolver;
using EightQueensRobot.Utilities;

namespace NQueensSolverTests.FireflyAlgorithmTestSolver;

public class FireflyGenericSolver(IFireflyIterationExitCriteriaHandler exitCriteriaHandler,
    IRandomNumberGenerator randomNumberGenerator,
    IFireflySwarmHandler<float[], Vector3> swarmHandler)
{
    public float[] GetInputsForOutput(Vector3 output)
    {
        int iterationCount = 0;
        Firefly<float[], Vector3>[] swarm = GenerateInitialSwarm();
        swarmHandler.ProcessSwarm(swarm, output);
        while (!exitCriteriaHandler.CanStopIterating())
        {
            iterationCount++;
            Console.WriteLine($"Iteration: {iterationCount}");
            swarmHandler.ConcentrateSwarm();
            
        }
        Firefly<float[], Vector3> bestChoice = swarmHandler.GetClosestFirefly();
        return bestChoice.Data;
    }

    private Firefly<float[], Vector3>[] GenerateInitialSwarm()
    {
        List<Firefly<float[], Vector3>> swarm = [];
        
        for (int i = 0; i < swarmHandler.GetSwarmSize(); i++)
        {
            swarm.Add(GenerateFirefly());    
        }
        
        return swarm.ToArray();
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