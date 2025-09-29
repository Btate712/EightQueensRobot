using EightQueensRobot.FKSolver;
using EightQueensRobot.IKSolver;

namespace EightQueensRobot.RobotModel;

public interface ISixDofRobotModel : IRobotAngleConstraints
{
    DhChain DhChain { get; }
}