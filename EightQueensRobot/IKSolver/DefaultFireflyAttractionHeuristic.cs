using System.Numerics;
using EightQueensRobot.RobotModel;
using EightQueensRobot.Utilities;

namespace EightQueensRobot.IKSolver;

public class DefaultFireflyAttractionHeuristic(IRandomNumberGenerator randomNumberGenerator, ISixDofRobotModel robotModel) : IFireflyAttractionHeuristic<SixDofJointData, Vector3>
{
    private const float Alpha = 0.3f;
    private const float Beta = 0.9f;
    private const float Gamma = 0.9f;

    public void MoveFirefly(Firefly<SixDofJointData, Vector3> fireflyToMove, Firefly<SixDofJointData, Vector3> brighterNeighbor)
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
    
    private double GetRSquared(Firefly<SixDofJointData, Vector3> fireflyToMove,
        Firefly<SixDofJointData, Vector3> brighterNeighbor)
    {
        double joint1Distance = fireflyToMove.Data.Joint1 - brighterNeighbor.Data.Joint1;
        double joint2Distance = fireflyToMove.Data.Joint2 - brighterNeighbor.Data.Joint2;
        double joint3Distance = fireflyToMove.Data.Joint3 - brighterNeighbor.Data.Joint3;
        double joint4Distance = fireflyToMove.Data.Joint4 - brighterNeighbor.Data.Joint4;
        double joint5Distance = fireflyToMove.Data.Joint5 - brighterNeighbor.Data.Joint5;
        double joint6Distance = fireflyToMove.Data.Joint6 - brighterNeighbor.Data.Joint6;

        return joint1Distance * joint1Distance +
            joint2Distance * joint2Distance +
            joint3Distance * joint3Distance +
            joint4Distance * joint4Distance +
            joint5Distance * joint5Distance +
            joint6Distance * joint6Distance;
    }

    private void MoveToNewPosition(Firefly<SixDofJointData, Vector3> fireflyToMove,
        Firefly<SixDofJointData, Vector3> brighterNeighbor, double localBeta, float randomizationValue)
    {
        float newJoint1 = GetNewValue(fireflyToMove.Data.Joint1, brighterNeighbor.Data.Joint1, localBeta, randomizationValue);
        newJoint1 = GetClampedValue(value: newJoint1, min: robotModel.Axis1MinAngle, max: robotModel.Axis1MaxAngle);
        
        float newJoint2 = GetNewValue(fireflyToMove.Data.Joint2, brighterNeighbor.Data.Joint2, localBeta, randomizationValue);
        newJoint2 = GetClampedValue(value: newJoint2, min: robotModel.Axis2MinAngle, max: robotModel.Axis2MaxAngle);
        
        float newJoint3 = GetNewValue(fireflyToMove.Data.Joint3, brighterNeighbor.Data.Joint3, localBeta, randomizationValue);
        newJoint3 = GetClampedValue(value: newJoint3, min: robotModel.Axis3MinAngle, robotModel.Axis3MaxAngle);
        
        float newJoint4 = GetNewValue(fireflyToMove.Data.Joint4, brighterNeighbor.Data.Joint4, localBeta, randomizationValue);
        newJoint4 = GetClampedValue(value: newJoint4, min: robotModel.Axis4MinAngle, robotModel.Axis4MaxAngle);
        
        float newJoint5 = GetNewValue(fireflyToMove.Data.Joint5, brighterNeighbor.Data.Joint5, localBeta, randomizationValue);
        newJoint5 = GetClampedValue(value: newJoint5, min: robotModel.Axis5MinAngle, robotModel.Axis5MaxAngle);
        
        float newJoint6 = GetNewValue(fireflyToMove.Data.Joint6, brighterNeighbor.Data.Joint6, localBeta, randomizationValue);
        newJoint6 = GetClampedValue(value: newJoint6, min: robotModel.Axis6MinAngle, robotModel.Axis6MaxAngle);
        
        SixDofJointData data = new(newJoint1, newJoint2, newJoint3, newJoint4, newJoint5, newJoint6);
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
    
    private float GetNewValue(float value1, float value2, double localBeta, float randomizationValue)
    {
        return (float) (value1 + localBeta * (value2 - value1) +
                        Alpha * randomizationValue);
    }
}