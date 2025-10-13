using EightQueensRobot.FKSolver;

namespace EightQueensRobot.RobotModel;

public class AbbIrb120 : IRobotModel
{
    private DhChain? _dhChain;
    private const float MaxReachLimit = 0.85f;
    
    private static readonly JointParameters Joint1 = new(
        JointType: JointType.Revolute,
        D: 0.291f,
        A: -1.57079f,
        Alpha: -1.57079f,
        AxisOffset: 0.0f,
        MinAngle: -2.87979f,
        MaxAngle: 2.87979f,
        RotationalSpeed: 250f);

    private static readonly JointParameters Joint2 = new(
        JointType: JointType.Revolute,
        D: 0.0f,
        A: 0.270f,
        Alpha: 0.0f,
        AxisOffset: -1.57079f,
        MinAngle: -1.91986f,
        MaxAngle: 1.91986f,
        RotationalSpeed: 250f);

    private static readonly JointParameters Joint3 = new(
        JointType: JointType.Revolute,
        D: 0.0f,
        A: 0.070f,
        Alpha: -1.57079f,
        AxisOffset: 0.0f,
        MinAngle: -1.91986f,
        MaxAngle: 1.22173f,
        RotationalSpeed: 250f);

    private static readonly JointParameters Joint4 = new(
        JointType: JointType.Revolute,
        D: 0.302f,
        A: 0.0f,
        Alpha: 1.57079f,
        AxisOffset: 0.0f,
        MinAngle: -2.79253f,
        MaxAngle: 2.79253f,
        RotationalSpeed: 320f);

    private static readonly JointParameters Joint5 = new(
        JointType: JointType.Revolute,
        D: 0.0f,
        A: 0.0f,
        Alpha: -1.57079f,
        AxisOffset: 0.0f,
        MinAngle: -2.09439f,
        MaxAngle: 2.09439f,
        RotationalSpeed: 320f);
    
    private static readonly JointParameters Joint6 = new(
        JointType: JointType.Revolute,
        D: 0.072f,
        A: 0.0f,
        Alpha: 0.0f,
        AxisOffset: 3.14159f,
        MinAngle: -6.98132f,
        MaxAngle: 6.98132f,
        RotationalSpeed: 420f);

    private static readonly JointParameters[] Joints =
    [
        Joint1, Joint2, Joint3, Joint4, Joint5, Joint6
    ];

    public int GetDoF()
    {
        return Joints.Length;
    }

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

    public float MaxReach => Joints.Select(j => j.A).Sum() *  MaxReachLimit;

    private int GetJointIndex(int jointNumber)
    {
        if (jointNumber > Joints.Length || jointNumber < 1)
        {
            throw new IndexOutOfRangeException();
        }

        return jointNumber - 1;
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
}