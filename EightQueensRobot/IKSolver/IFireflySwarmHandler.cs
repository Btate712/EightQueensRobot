using System.Numerics;

namespace EightQueensRobot.IKSolver;

public interface IFireflySwarmHandler<TInput, TOutput> where TInput : class
{
    Firefly<TInput, TOutput> GetClosestFirefly();
    Firefly<TInput, TOutput>[] GenerateFireflySwarm();
    void ProcessSwarm(Firefly<TInput, TOutput>[] firefly, Vector3 targetPosition);
    void ConcentrateSwarm();
    int GetSwarmSize();
}