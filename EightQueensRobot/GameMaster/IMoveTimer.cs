using EightQueensRobot.RobotModel;

namespace EightQueensRobot.GameMaster;

public interface IMoveTimer
{
    float CalculateMoveTime(JointAngles initialAngles, JointAngles finalAngles);
}