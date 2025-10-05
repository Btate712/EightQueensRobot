namespace EightQueensRobot.PuzzleSolver;

public interface IPuzzleSolver<TMoveSchema>
{
    TMoveSchema GetNextMove(TMoveSchema lastMove);
    TMoveSchema DefaultStartPosition { get; }
    TMoveSchema SolvedResponse { get; }
}