using EightQueensRobot.FKSolver;
using EightQueensRobot.IKSolver;

namespace EightQueensRobot.RobotModel;

public interface IRobotModel : IRobotAngleConstraints
{
    DhChain DhChain { get; }
    float GetRotationalSpeed(int jointNumber);
    int GetDoF();
    float RSquaredNormalizationValue { get; }
    float MaxReach { get; }
}