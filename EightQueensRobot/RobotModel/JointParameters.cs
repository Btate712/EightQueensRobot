using EightQueensRobot.FKSolver;

namespace EightQueensRobot.RobotModel;

public record JointParameters(
    JointType JointType,
    float D,
    float A,
    float Alpha,
    float AxisOffset,
    float MinAngle,
    float MaxAngle,
    float RotationalSpeed);