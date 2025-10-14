using System.Numerics;

namespace EightQueensRobot.IKSolver;

public interface IFireflyCache<TData, TOutput> where TData : class
{
    JointAngleBoundaries GetCachedBoundaries(Vector3 location);
    void CacheSwarm(Firefly<TData, TOutput>[] firefly);
    bool HasCachedValue(TOutput location);
}