namespace EightQueensRobot.IKSolver;

public interface IIkSolverFactory<out TSolverOutput>
{
    IIkSolver<TSolverOutput> GetDefaultIkSolver();
}