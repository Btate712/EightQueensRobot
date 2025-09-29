using System.Numerics;

namespace EightQueensRobot.IKSolver;

public interface IIkSolver<out TOutput>
{
    TOutput GetJointAnglesForPosition(Vector3 position);
}