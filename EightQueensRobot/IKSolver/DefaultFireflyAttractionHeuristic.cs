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
        double localBeta = GetLocalBeta(r2);
        float randomizationValue = GetRandomizationValue();
        MoveToNewPosition(
            fireflyToMove: fireflyToMove, 
            brighterNeighbor: brighterNeighbor, 
            localBeta: localBeta, 
            randomizationValue: randomizationValue);
    }

    private double GetLocalBeta(double r2)
    {
        return Beta * Math.Exp(-Gamma * r2);    
    }

    private float GetRandomizationValue()
    {
        const float min = -0.5f;
        const float max = 0.5f;
        return randomNumberGenerator.GetRandomNumberBetween(min, max);
    }
    
    private double GetRSquared(Firefly<JointAngles, Vector3> fireflyToMove,
        Firefly<JointAngles, Vector3> brighterNeighbor)
    {
        double xDistance = fireflyToMove.Output.X - brighterNeighbor.Output.X;
        double yDistance = fireflyToMove.Output.Y - brighterNeighbor.Output.Y;
        double zDistance = fireflyToMove.Output.Z - brighterNeighbor.Output.Z;

        return xDistance * xDistance +
               yDistance * yDistance +
               zDistance * zDistance;
    }

    private void MoveToNewPosition(Firefly<JointAngles, Vector3> fireflyToMove,
        Firefly<JointAngles, Vector3> brighterNeighbor, double localBeta, float randomizationValue)
    {
        double newJoint1 = GetNewValue(fireflyToMove.Data.GetJoint(1), brighterNeighbor.Data.GetJoint(1), localBeta, randomizationValue);
        newJoint1 = GetClampedValue(value: newJoint1, min: robotModel.Axis1MinAngle, max: robotModel.Axis1MaxAngle);
        
        double newJoint2 = GetNewValue(fireflyToMove.Data.GetJoint(2), brighterNeighbor.Data.GetJoint(2), localBeta, randomizationValue);
        newJoint2 = GetClampedValue(value: newJoint2, min: robotModel.Axis2MinAngle, max: robotModel.Axis2MaxAngle);
        
        double newJoint3 = GetNewValue(fireflyToMove.Data.GetJoint(3), brighterNeighbor.Data.GetJoint(3), localBeta, randomizationValue);
        newJoint3 = GetClampedValue(value: newJoint3, min: robotModel.Axis3MinAngle, robotModel.Axis3MaxAngle);
        
        double newJoint4 = GetNewValue(fireflyToMove.Data.GetJoint(4), brighterNeighbor.Data.GetJoint(4), localBeta, randomizationValue);
        newJoint4 = GetClampedValue(value: newJoint4, min: robotModel.Axis4MinAngle, robotModel.Axis4MaxAngle);
        
        double newJoint5 = GetNewValue(fireflyToMove.Data.GetJoint(4), brighterNeighbor.Data.GetJoint(4), localBeta, randomizationValue);
        newJoint5 = GetClampedValue(value: newJoint5, min: robotModel.Axis5MinAngle, robotModel.Axis5MaxAngle);
        
        double newJoint6 = GetNewValue(fireflyToMove.Data.GetJoint(4), brighterNeighbor.Data.GetJoint(4), localBeta, randomizationValue);
        newJoint6 = GetClampedValue(value: newJoint6, min: robotModel.Axis6MinAngle, robotModel.Axis6MaxAngle);

        JointAngles data = new([newJoint1, newJoint2, newJoint3, newJoint4, newJoint5, newJoint6]);
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
    
    private float GetNewValue(double value1, double value2, double localBeta, float randomizationValue)
    {
        return (float) (value1 + localBeta * (value2 - value1) +
                        Alpha * randomizationValue);
    }
}