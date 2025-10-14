using System.Numerics;

namespace EightQueensRobot.IKSolver;

public class NullFireflyCache<TData, TOutput> : IFireflyCache<TData, TOutput> where TData : class
{
    public void CacheSwarm(Firefly<TData, TOutput>[] firefly)
    {
        // no caching behavior for null cache
    }

    public bool HasCachedValue(TOutput output)
    {
        return false;
    }

    JointAngleBoundaries IFireflyCache<TData, TOutput>.GetCachedBoundaries(Vector3 location)
    {
        return JointAngleBoundaries.Null;
    }
}