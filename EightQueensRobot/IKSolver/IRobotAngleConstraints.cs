namespace EightQueensRobot.IKSolver;

public interface IRobotAngleConstraints
{
    float Axis1MinAngle { get; } 
    float Axis1MaxAngle { get; }
    float Axis2MinAngle { get; }
    float Axis2MaxAngle { get; }
    float Axis3MinAngle { get; }
    float Axis3MaxAngle { get; }
    float Axis4MinAngle { get; }
    float Axis4MaxAngle { get; }
    float Axis5MinAngle { get; }
    float Axis5MaxAngle { get; }
    float Axis6MinAngle { get; }
    float Axis6MaxAngle { get; }
}