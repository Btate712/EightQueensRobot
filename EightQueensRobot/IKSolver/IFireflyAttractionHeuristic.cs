namespace EightQueensRobot.IKSolver;

public interface IFireflyAttractionHeuristic<TFireflyData, TTarget> where TFireflyData : class
{
    void MoveFirefly(
        Firefly<TFireflyData, TTarget> fireflyToMove, 
        Firefly<TFireflyData, TTarget> brighterNeighbor);
}