namespace EightQueensRobot.PuzzleSolver;

public interface IPuzzleSolver<TMoveSchema>
{
    TMoveSchema GetNextMove(TMoveSchema lastMove);
}