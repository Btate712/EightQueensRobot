namespace EightQueensRobot.IKSolver;

public class DefaultSwarmSizeHandler(int defaultSwarmSize) : ISwarmSizeHandler
{
    public int GetSwarmSize()
    {
        return defaultSwarmSize;
    }
}