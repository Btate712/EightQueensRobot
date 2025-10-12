using System.Numerics;
using EightQueensRobot.RobotModel;
using EightQueensRobot.Utilities;

namespace EightQueensRobot.IKSolver;

public class DefaultFireflyAttractionHeuristic(IRandomNumberGenerator randomNumberGenerator, IRobotModel robotModel) : IFireflyAttractionHeuristic<JointAngles, Vector3>
{
    private const float Alpha = 0.3f;
    private const float Beta = 0.9f;
    private const float Gamma = 0.9f;

    public void MoveFirefly(Firefly<JointAngles, Vector3> fireflyToMove, Firefly<JointAngles, Vector3> brighterNeighbor)
    {
        double r2 = GetRSquared(fireflyToMove,  brighterNeighbor);
        MoveToNewPosition(
            fireflyToMove: fireflyToMove, 
            brighterNeighbor: brighterNeighbor, 
            rSquared: r2);
    }

    private float GetRandomizationValue()
    {
        const float min = -0.005f;
        const float max = 0.005f;
        return randomNumberGenerator.GetRandomNumberBetween(min, max);
    }
    
    private double GetRSquared(Firefly<JointAngles, Vector3> fireflyToMove,
        Firefly<JointAngles, Vector3> brighterNeighbor)
    {
        float normalizationValue = robotModel.RSquaredNormalizationValue;
        
        float sum = (float)fireflyToMove.Data.AsArray
            .Select((jointAngle, index) => jointAngle - brighterNeighbor.Data.AsArray[index])
                .Sum(distance => distance * distance);
        
        return sum / normalizationValue;
    }

    private void MoveToNewPosition(Firefly<JointAngles, Vector3> fireflyToMove,
        Firefly<JointAngles, Vector3> brighterNeighbor, double rSquared)
    {
        List<double> angles = [];

        for (int joint = 1; joint <= robotModel.GetDoF(); joint++)
        {
            float randomizationValue = GetRandomizationValue();
            double newJoint = GetNewValue(fireflyToMove.Data.GetJoint(joint), brighterNeighbor.Data.GetJoint(joint), rSquared: rSquared, randomizationValue: randomizationValue);
            newJoint = GetClampedValue(value: newJoint, min: robotModel.GetMinAngle(joint),
                max: robotModel.GetMaxAngle(joint));
            
            angles.Add(newJoint);
        }
        
        JointAngles data = new(angles.ToArray());
        fireflyToMove.Data = data;
    }

    private double GetClampedValue(double value, float min, float max)
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