using System.Numerics;
using EightQueensRobot.RobotModel;

namespace EightQueensRobot.IKSolver;

public interface IFireflyCache<TData, TOutput> where TData : class
{
    JointAngleBoundaries GetCachedValues(Vector3 location);
    void CacheSwarm(Firefly<TData, TOutput> firefly);
}