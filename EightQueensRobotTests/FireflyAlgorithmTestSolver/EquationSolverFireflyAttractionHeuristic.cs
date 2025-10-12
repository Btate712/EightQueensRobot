using System.Numerics;
using EightQueensRobot.IKSolver;
using EightQueensRobot.Utilities;

namespace NQueensSolverTests.FireflyAlgorithmTestSolver;

public class EquationSolverFireflyAttractionHeuristic(IRandomNumberGenerator randomNumberGenerator) : IFireflyAttractionHeuristic<float[], Vector3>
{
    private const float Alpha = 0.3f;
    private const float Beta = 0.9f;
    private const float Gamma = 0.9f;
    
    public void MoveFirefly(Firefly<float[], Vector3> fireflyToMove, Firefly<float[], Vector3> brighterNeighbor)
    {
        float r2 = GetRSquared(fireflyToMove, brighterNeighbor);
        MoveToNewPosition(
            fireflyToMove: fireflyToMove, 
            brighterNeighbor: brighterNeighbor, 
            rSquared: r2);
    }

    private float GetRandomizationValue()
    {
        const float min = -0.5f;
        const float max = 0.5f;
        return randomNumberGenerator.GetRandomNumberBetween(min, max);
    }
    
    private float GetRSquared(Firefly<float[], Vector3> fireflyToMove,
        Firefly<float[], Vector3> brighterNeighbor)
    {
        // 6 dimensions * range (200) squared.
        const int normalizationValue = 240_000;
        
        float sum = fireflyToMove.Data
            .Select((dataPoint, i) => dataPoint - brighterNeighbor.Data[i])
            .Sum(distance => distance * distance);

        return sum / normalizationValue;
    }
    
    private void MoveToNewPosition(Firefly<float[], Vector3> fireflyToMove,
        Firefly<float[], Vector3> brighterNeighbor, double rSquared)
    {
        List<float> variables = [];

        for (int variable = 0; variable < 6; variable++)
        {
            float randomizationValue = GetRandomizationValue();
            float newVariables = GetNewValue(fireflyToMove.Data[variable], brighterNeighbor.Data[variable], rSquared, randomizationValue);
            newVariables = GetClampedValue(value: newVariables, min: -100,
                max: 100);
            
            variables.Add(newVariables);
        }
        
        float[] data = variables.ToArray();
        fireflyToMove.Data = data;
    }

    private float GetClampedValue(float value, float min, float max)
    {
        if (min >= max)
        {
            throw new ArgumentOutOfRangeException(nameof(min), min, $"Min value must be between {min} and {max}");
        }
        
        if (value < min)
        {
            return min;
        }

        if (value > max)
        {
            return max;
        }
        
        return value;
    }
    
    private float GetNewValue(double value1, double value2, double rSquared, float randomizationValue)
    {
        return (float) (
            value1 + 
            Beta * Math.Exp(-Gamma * rSquared) * (value2 - value1) + 
            Alpha * randomizationValue);
    }
}