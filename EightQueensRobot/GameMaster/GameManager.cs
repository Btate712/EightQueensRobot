using System.Numerics;
using EightQueensRobot.PuzzleSolver;
using EightQueensRobot.Reporting;

namespace EightQueensRobot.GameMaster;

public class GameManager(
    IPuzzleSolver<string> puzzleSolver,
    BoardManager boardManager,
    IMoveController moveController,
    QueenPositionManager queenPositionManager,
    IDataOutput dataOutput)
{
    private readonly Vector3 _hopperPosition = new(-0.010f, -0.010f, 0);
    
    public void Run()
    {
        bool solved = false;
        string lastMove = puzzleSolver.DefaultStartPosition;
        
        while (!solved)
        {
            string nextMove = puzzleSolver.GetNextMove(lastMove);
            if (nextMove == puzzleSolver.SolvedResponse)
            {
                solved = true;
            }
            else
            {
                lastMove = nextMove;
                QueenMove[] moves = queenPositionManager.GetMovesToAchieve(nextMove);
                foreach (QueenMove move in moves)
                {
                    Console.WriteLine("Moving...");
                    Vector3 startPosition = move.Source == QueenPosition.Hopper 
                        ? _hopperPosition 
                        : boardManager.GetSquareCenter(move.Source.X, move.Source.Y);
                    
                    MoveData moveData = new(move, startPosition);
                    moveController.Move(moveData);
                    moveController.Grab();

                    Vector3 endPosition = move.Destination == QueenPosition.Hopper
                        ? _hopperPosition 
                        : boardManager.GetSquareCenter(move.Destination.X, move.Destination.Y);
                    
                    MoveData moveData2 = new(move, endPosition);
                    moveController.Move(moveData2);
                    moveController.Release();
                }
            }
        }
        
        dataOutput.WriteData();
    }
}   