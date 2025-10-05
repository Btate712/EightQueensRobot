using EightQueensRobot.Utilities;

namespace EightQueensRobot.PuzzleSolver;

public class NQueensSolver(int size) : IPuzzleSolver<string>
{
    public const string Solved = "Solved";
    private const string EmptyBoardMove = "00000000";
    private readonly NQueensMoveStringValidator _moveValidator = new(size);
    private readonly int[] _lastMoveRows = new int[size];
    private string _lastMove = EmptyBoardMove;
    private int _firstRowWithOffendingQueen;
    private bool _collisionFound;
    private bool _checked;
    
    public const string InvalidMove = "Invalid move";
    public string SolvedResponse => "Solved";
    
    public string GetNextMove(string lastMove)
    {
        ResetSolverForMove(lastMove);
        if (!_moveValidator.IsValidMove(_lastMove))
        {
            return InvalidMove;
        }
        
        SetRows();
        
        if (IsSolved())
        {
            return Solved;
        }
        
        return FindNextMove();
    }

    public string DefaultStartPosition => EmptyBoardMove;

    private void ResetSolverForMove(string lastMove)
    {
        _collisionFound = false;
        _checked = false;
        _firstRowWithOffendingQueen = -1;
        _lastMove = lastMove;
    }
    
    private void SetRows()
    {
        for (int i = 0; i < _lastMove.Length; i++)
        {
            _lastMoveRows[i] = _lastMove[i].AsInt();
        }
    }
    
    private bool IsSolved()
    {
        return !AnyRowHasNoQueen() && !TwoQueensCanAttackOneAnother();
    }

    private bool AnyRowHasNoQueen()
    {
        return _lastMoveRows.Any(c => c == 0);
    }

    private bool TwoQueensCanAttackOneAnother()
    {
        if (_checked)
        {
            return _collisionFound;
        }

        bool collisionFound = HasTwoQueensInSameColumn() || HasTwoQueensThatCanAttackDiagonally();
        _checked = true;
        return collisionFound;
    }
    
    private bool HasTwoQueensInSameColumn()
    {
        int firstOffendingRow = FindFirstRowWithSecondQueenInSameColumn();
        if (firstOffendingRow == -1)
        {
            return false;
        }

        _firstRowWithOffendingQueen = firstOffendingRow;
        _collisionFound = true;
        return true;
    }

    private int FindFirstRowWithSecondQueenInSameColumn()
    {
        int[] filledRows = _lastMoveRows.Where(c => c != 0).ToArray();
        bool[] columnUsed = new bool[size + 1];
        
        for (int i = 0; i < filledRows.Length; i++)
        {
            if (columnUsed[filledRows[i]])
            {
                return i;
            }

            columnUsed[filledRows[i]] = true;
        }

        return -1;
    }
    
    private bool HasTwoQueensThatCanAttackDiagonally()
    {
        int firstOffendingRow = FindFirstRowThatAddsDiagonallyAttackingQueen();
        if (firstOffendingRow == -1)
        {
            return false;
        }

        _firstRowWithOffendingQueen = firstOffendingRow;
        return true;
    }

    private int FindFirstRowThatAddsDiagonallyAttackingQueen()
    {
        int[] filledRows = _lastMoveRows.Where(c => c != 0).ToArray();
        List<int> leftDiagonalThreatenedColumns = [];
        List<int> rightDiagonalThreatenedColumns = [];
        
        for (int i = 0; i < filledRows.Length; i++)
        {
            if (leftDiagonalThreatenedColumns.Contains(filledRows[i]) ||
                rightDiagonalThreatenedColumns.Contains(filledRows[i]))
            {
                _collisionFound = true;
                return i;
            }
            
            // We don't have to worry about diagonals extending past the edge of the board because there will never be a
            // filled row with a column value outside the edges of the board.
            leftDiagonalThreatenedColumns = leftDiagonalThreatenedColumns.Select(c => c - 1).ToList();
            rightDiagonalThreatenedColumns = rightDiagonalThreatenedColumns.Select(c => c + 1).ToList();
            leftDiagonalThreatenedColumns.Add(filledRows[i] - 1);
            rightDiagonalThreatenedColumns.Add(filledRows[i] + 1);
        }

        return -1;
    }
    
    private string FindNextMove()
    {
        if (TwoQueensCanAttackOneAnother())
        {
            return IncrementLastPlacedQueenPosition();
        }

        if (IsSolved())
        {
            return Solved;
        }
        
        return PlaceQueenInNextEmptyRow();
    }

    private string PlaceQueenInNextEmptyRow()
    {
        int nextEmptyRowIndex = Array.FindIndex(_lastMoveRows, c => c == 0);
        _lastMoveRows[nextEmptyRowIndex] = 1;
        return StringifyRows(_lastMoveRows);
    }

    private string IncrementLastPlacedQueenPosition()
    {
        RemoveAllQueensAfterFirstOffendingRow();
        int nextEmptyRowIndex = Array.FindIndex(_lastMoveRows, c => c == 0);
        
        // If no empty row is found, the last placed queen is in the last row.
        if (nextEmptyRowIndex == -1)
        {
            nextEmptyRowIndex = size;
        }

        IncrementColumnForRow(nextEmptyRowIndex - 1);
        
        return StringifyRows(_lastMoveRows);
    }

    private void IncrementColumnForRow(int row)
    {
        if (_lastMoveRows[row] == size)
        {
            Backtrack(row);
        }
        
        _lastMoveRows[row]++;
    }

    private void Backtrack(int row)
    {
        if (row == 0)
        {
            _lastMoveRows[row] = 1;
        }
        else
        {
            _lastMoveRows[row] = 0;
            IncrementColumnForRow(row - 1);
        }
    }
    
    private string StringifyRows(int[] rows)
    {
        return string.Join("", rows);
    }

    private void RemoveAllQueensAfterFirstOffendingRow()
    {
        for (int i = _firstRowWithOffendingQueen + 1; i < size; i++)
        {
            _lastMoveRows[i] = 0;
        }
    }
}