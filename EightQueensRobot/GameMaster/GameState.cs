using EightQueensRobot.IKSolver;
using EightQueensRobot.RobotModel;

namespace EightQueensRobot.GameMaster;

public class GameState
{
    public Firefly<SixDofJointData>[] CurrentSwarm { get; set; } = [];
}
