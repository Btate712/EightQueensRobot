namespace EightQueensRobot.IKSolver;

public interface IRobotAngleConstraints
{
    float GetMinAngle(int jointNumber);
    float GetMaxAngle(int jointNumber);
}