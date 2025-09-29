using EightQueensRobot.FKSolver;

namespace EightQueensRobot.RobotModel;

public class AbbIrb120 : ISixDofRobotModel
{
    private DhChain? _dhChain;
    
    // Standard DH Parameters
    // length values in meters
    private const float D1 = 0.290f;
    private const float D2 = 0.0f;
    private const float D3 = 0.0f;
    private const float D4 = 0.302f;
    private const float D5 = 0.0f;
    private const float D6 = 0.072f;

    private const float A1 = 0.0f;
    private const float A2 = 0.270f;
    private const float A3 = 0.070f;
    private const float A4 = 0.0f;
    private const float A5 = 0.0f;
    private const float A6 = 0.0f;

    // angle values in radians
    private const float Alpha1 = -1.57079f;
    private const float Alpha2 = 0.0f;
    private const float Alpha3 = -1.57079f;
    private const float Alpha4 = 1.57079f;
    private const float Alpha5 = -1.57079f;
    private const float Alpha6 = 0.0f;
    
    private const float Axis1Offset = 0.0f;
    private const float Axis2Offset = -1.57079f;
    private const float Axis3Offset = 0.0f; 
    private const float Axis4Offset = 0.0f; 
    private const float Axis5Offset = 0.0f; 
    private const float Axis6Offset = 3.14159f; 

    public float Axis1MinAngle => -2.87979f;
    public float Axis1MaxAngle => 2.87979f;
    public float Axis2MinAngle => -1.91986f;
    public float Axis2MaxAngle  => 1.91986f;
    public float Axis3MinAngle => -1.91986f;
    public float Axis3MaxAngle => 1.22173f;
    public float Axis4MinAngle => -2.79253f;
    public float Axis4MaxAngle => 2.79253f;
    public float Axis5MinAngle  => -2.09439f;
    public float Axis5MaxAngle => 2.09439f;
    public float Axis6MinAngle => -6.98132f;
    public float Axis6MaxAngle => 6.98132f;

    public DhChain DhChain
    {
        get
        {
            _dhChain ??= BuildDhChain();

            return _dhChain;
        }
    }

    private DhChain BuildDhChain()
    {
        return new DhChain(
            [
                new DhLink(
                    jointType: JointType.Revolute,
                    a: A1,
                    alpha: Alpha1,
                    d: D1,
                    theta: 0,
                    offset: Axis1Offset,
                    min: Axis1MinAngle,
                    max: Axis1MaxAngle),
                new DhLink(
                    jointType: JointType.Revolute,
                    a: A2,
                    alpha: Alpha2,
                    d: D2,
                    theta: 0,
                    offset: Axis2Offset,
                    min: Axis2MinAngle,
                    max: Axis2MaxAngle),
                new DhLink(
                    jointType: JointType.Revolute,
                    a: A3,
                    alpha: Alpha3,
                    d: D3,
                    theta: 0,
                    offset: Axis3Offset,
                    min: Axis3MinAngle,
                    max: Axis3MaxAngle),
                new DhLink(
                    jointType: JointType.Revolute,
                    a: A4,
                    alpha: Alpha4,
                    d: D4,
                    theta: 0,
                    offset: Axis4Offset,
                    min: Axis4MinAngle,
                    max: Axis4MaxAngle),
                new DhLink(
                    jointType: JointType.Revolute,
                    a: A5,
                    alpha: Alpha5,
                    d: D5,
                    theta: 0,
                    offset: Axis5Offset,
                    min: Axis5MinAngle,
                    max: Axis5MaxAngle),
                new DhLink(
                    jointType: JointType.Revolute,
                    a: A6,
                    alpha: Alpha6,
                    d: D6,
                    theta: 0,
                    offset: Axis6Offset,
                    min: Axis6MinAngle,
                    max: Axis6MaxAngle)
            ]
        );
    }
}