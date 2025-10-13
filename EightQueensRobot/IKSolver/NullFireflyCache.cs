using System.Numerics;

namespace EightQueensRobot.IKSolver;

public class NullFireflyCache<TData, TOutput> : IFireflyCache<TData, TOutput> where TData : class
{
    public void CacheSwarm(Firefly<TData, TOutput> firefly)
    {
        // no caching behavior for null cache
    }

    JointAngleBoundaries IFireflyCache<TData, TOutput>.GetCachedValues(Vector3 location)
    {
        return JointAngleBoundaries.Null;
    }
}