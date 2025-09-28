namespace EightQueensRobot.PuzzleSolver;

public class NQueensMoveStringValidator(int numberOfRows)
{
    public bool IsValidMove(string move)
    {
        if (move.Length != numberOfRows)
        {
            return false;
        }

        foreach (char character in move)
        {
            bool isCharacter = int.TryParse(character.ToString(), out int col);
            if (!isCharacter)
            {
                return false;
            }

            if (col > NumberOfColumns)
            {
                return false;
            }
        }

        if (HasAnEmptyRowAboveANonEmptyRow(move))
        {
            return false;
        }
        
        return true;
    }

    private bool HasAnEmptyRowAboveANonEmptyRow(string move)
    {
        const char empty = '0';
        
        bool emptyRowFound = false;
        for (int i = 0; i < NumberOfColumns; i++)
        {
            if (move[i] == empty)
            {
                emptyRowFound = true;
            }
            else
            {
                if (emptyRowFound)
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    // Validator assumes a square chessboard, so number of columns == number of rows
    private int NumberOfColumns => numberOfRows;
}