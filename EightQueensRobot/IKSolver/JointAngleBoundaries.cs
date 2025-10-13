using System.Numerics;
using EightQueensRobot.RobotModel;

namespace EightQueensRobot.IKSolver;

public record JointAngleBoundaries
{
    private readonly JointAngles _minValues;
    private readonly JointAngles _maxValues;

    public static readonly JointAngleBoundaries Null = new();
    
    public JointAngleBoundaries(JointAngles minValues, JointAngles maxValues)
    {
        if (minValues == maxValues || minValues.AsArray.Length == 0 || maxValues.AsArray.Length == 0)
        {
            throw new ArgumentException("Cannot initialize boundaries with empty values");
        }

        _minValues = minValues;
        _maxValues = maxValues;
    }

    public float GetMinValueForJoint(int joint)
    {
        return (float)_minValues.GetJoint(joint);
    }

    public float GetMaxValueForJoint(int joint)
    {
        return (float)_maxValues.GetJoint(joint);
    }

    private JointAngleBoundaries()
    {
        _minValues = new JointAngles([]);
        _maxValues = new JointAngles([]);
    }

    public bool IsNull => this == Null;
}