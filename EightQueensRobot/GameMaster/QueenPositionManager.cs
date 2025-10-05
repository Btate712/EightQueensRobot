using EightQueensRobot.Reporting;

namespace EightQueensRobot.GameMaster;

public class QueenPositionManager
{
    private readonly int _boardSize;
    private readonly bool[,] _board;
    
    public QueenPositionManager(int boardSize)
    {
        _boardSize = boardSize;
        _board = new bool[boardSize, boardSize];
        InitializeBoard();
    }

    public QueenMove[] GetMovesToAchieve(string queenPositions)
    {
        List<QueenMove> moves = [];
        List<QueenPosition> queensToRemove = [];
        List<QueenPosition> queensToAdd = [];

        for (int i = 0; i < queenPositions.Length; i++)
        {
            HandleRow(i, queenPositions, queensToRemove, queensToAdd);   
        }

        foreach (QueenPosition queenPosition in queensToRemove)
        {
            moves.Add(new QueenMove(queenPosition, QueenPosition.Hopper));
        }

        foreach (QueenPosition queenPosition in queensToAdd)
        {
            moves.Add(new QueenMove(QueenPosition.Hopper, queenPosition));
        }
        
        return moves.ToArray();
    }

    private void HandleRow(int zeroIndexedRow, string queenPositions, List<QueenPosition> queensToRemove, List<QueenPosition> queensToAdd)
    {
        QueenPosition queenPosition = GetDesiredBoardPosition(queenPositions, zeroIndexedRow);

        for (int col = 0; col < _boardSize; col++)
        {
            bool isQueen = _board[zeroIndexedRow, col];
            bool shouldBeQueen = queenPosition.X - 1 == zeroIndexedRow && queenPosition.Y - 1 == col;

            if (isQueen && !shouldBeQueen)
            {
                queensToRemove.Add(new QueenPosition(zeroIndexedRow + 1, col + 1));
                RemoveQueenFromBoard(new QueenPosition(zeroIndexedRow, col));
            }
            else if (!isQueen && shouldBeQueen)
            {
                queensToAdd.Add(new QueenPosition(zeroIndexedRow + 1, col + 1));
                AddQueenToBoard(new QueenPosition(zeroIndexedRow, col));
            }
        }
    }

    private void AddQueenToBoard(QueenPosition queenPosition)
    {
        _board[queenPosition.X, queenPosition.Y] = true;
    }

    private void RemoveQueenFromBoard(QueenPosition queenPosition)
    {
        _board[queenPosition.X, queenPosition.Y] = false;
    }
    
    private QueenPosition GetDesiredBoardPosition(string queenPositions, int zeroIndexedRow)
    {
        char columnChar = queenPositions[zeroIndexedRow];
        int oneIndexedColumn = columnChar - '0';
        int oneIndexedRow = zeroIndexedRow + 1;
        
        return new QueenPosition(oneIndexedRow, oneIndexedColumn);
    }
    
    private void InitializeBoard()
    {
        for (int i = 0; i < _boardSize; i++)
        {
            for (int j = 0; j < _boardSize; j++)
            {
                _board[i, j] = false;
            }
        }
    }
}