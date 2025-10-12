using EightQueensRobot.FKSolver;
using EightQueensRobot.Utilities;

namespace EightQueensRobot.RobotModel;

public class Sungur370 : IRobotModel
{
    private DhChain? _dhChain;
    
    private static readonly JointParameters Joint1 = new(
        JointType: JointType.Revolute,
        D: 0.5f,
        A: 0f,
        Alpha: -90f.ToRadians(),
        AxisOffset: 0f,
        MinAngle: -180f.ToRadians(),
        MaxAngle: 180f.ToRadians(),
        RotationalSpeed: 250f.ToRadians());    
    
    private static readonly JointParameters Joint2 = new(
        JointType: JointType.Revolute,
        D: 0f,
        A: 0.2f,
        Alpha: 90f.ToRadians(),
        AxisOffset: 0f,
        MinAngle: -90f.ToRadians(),
        MaxAngle: 30f.ToRadians(),
        RotationalSpeed: 250f.ToRadians());
        
    private static readonly JointParameters Joint3 = new(
        JointType: JointType.Revolute,
        D: 0f,
        A: 0.25f,
        Alpha: -90f.ToRadians(),
        AxisOffset: 0f,
        MinAngle: -90f.ToRadians(),
        MaxAngle: 120f.ToRadians(),
        RotationalSpeed: 250f.ToRadians());
    
    private static readonly JointParameters Joint4 = new(
        JointType: JointType.Revolute,
        D: 0f,
        A: 0.3f,
        Alpha: 90f.ToRadians(),
        AxisOffset: 0f,
        MinAngle: -90f.ToRadians(),
        MaxAngle: 90f.ToRadians(),
        RotationalSpeed: 250f.ToRadians());
    
    private static readonly JointParameters Joint5 = new(
        JointType: JointType.Revolute,
        D: 0f,
        A: 0.2f,
        Alpha: -90f.ToRadians(),
        AxisOffset: 0f,
        MinAngle: -90f.ToRadians(),
        MaxAngle: 90f.ToRadians(),
        RotationalSpeed: 250f.ToRadians());
    
    private static readonly JointParameters Joint6 = new(
        JointType: JointType.Revolute,
        D: 0f,
        A: 0.2f,
        Alpha: 0f,
        AxisOffset: 0f,
        MinAngle: -90f.ToRadians(),
        MaxAngle: 90f.ToRadians(),
        RotationalSpeed: 250f.ToRadians());
    
    private static readonly JointParameters Joint7 = new(
        JointType: JointType.Revolute,
        D: 0.05f,
        A: 0.1f,
        Alpha: 0f,
        AxisOffset: 0f,
        MinAngle: -30f.ToRadians(),
        MaxAngle: 90f.ToRadians(),
        RotationalSpeed: 250f.ToRadians());

    private static readonly JointParameters[] Joints = [
        Joint1, Joint2, Joint3, Joint4, Joint5, Joint6, Joint7
    ];
    
    public float GetMinAngle(int jointNumber)
    {
        int jointIndex = GetJointIndex(jointNumber);
        return Joints[jointIndex].MinAngle;
    }

    public float GetMaxAngle(int jointNumber)
    {
        int jointIndex = GetJointIndex(jointNumber);
        return Joints[jointIndex].MaxAngle;
    }

    public float GetRotationalSpeed(int jointNumber)
    {
        int jointIndex = GetJointIndex(jointNumber);
        return Joints[jointIndex].RotationalSpeed;
    }

    public DhChain DhChain
    {
        get
        {
            _dhChain ??= BuildDhChain();
            
            return _dhChain;
        }
    }

    public float RSquaredNormalizationValue
    {
        get
        {
            return Joints
                .Select(joint => joint.MaxAngle - joint.MinAngle)
                .Select(jointRange => jointRange * jointRange)
                .Sum();
        }
    }

    public int GetDoF()
    {
        return Joints.Length;
    }
    
    private DhChain BuildDhChain()
    {
        DhLink[] links = Joints.Select(j => new DhLink(
            jointType: j.JointType,
            a: j.A,
            alpha: j.Alpha,
            d: j.D,
            theta: 0,
            offset: j.AxisOffset,
            min: j.MinAngle,
            max: j.MaxAngle)).ToArray();
        
        return new DhChain(links);
    }
    
    private int GetJointIndex(int jointNumber)
    {
        if (jointNumber > Joints.Length || jointNumber < 1)
        {
            throw new IndexOutOfRangeException();
        }

        return jointNumber - 1;
    }
}