using System.Numerics;

namespace EightQueensRobot.IKSolver;

public interface IFireflySwarmHandler<TInput, TOutput>: ISwarmSizeHandler where TInput : class
{
    Firefly<TInput, TOutput> GetClosestFirefly();
    void ProcessSwarm(Firefly<TInput, TOutput>[] firefly, Vector3 targetPosition);
    void ConcentrateSwarm();
}