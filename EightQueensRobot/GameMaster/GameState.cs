using System.Numerics;
using EightQueensRobot.IKSolver;
using EightQueensRobot.RobotModel;

namespace EightQueensRobot.GameMaster;

public class GameState
{
    public Firefly<SixDofJointData, Vector3>[] CurrentSwarm { get; set; } = [];
}
