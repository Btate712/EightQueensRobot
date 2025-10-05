using System.Numerics;

namespace EightQueensRobot.GameMaster;

public class BoardManager
{
    private readonly int _boardSize;
    private readonly Vector3[][] _boardSquares;
    private readonly float _squareWidth;
    private readonly float _squareOffset;
    private readonly float _minX;
    private readonly float _minY;
    private readonly float _z;
    
    public BoardManager(int boardSize, Vector3 corner1, Vector3 corner2)
    {
        _boardSize = boardSize;
        
        if (Math.Abs(corner1.Z - corner2.Z) > 0.001)
        {
            throw new ArgumentException("Board must sit on a flat, even surface (corner Z values must match)");
        }

        if (Math.Abs(Math.Abs(corner1.X - corner2.X) - Math.Abs(corner1.Y - corner2.Y)) > 0.001)
        {
            throw new ArgumentException("Board must be square");
        }
        
        _boardSquares = new Vector3[_boardSize][];
        for (int i = 0; i < _boardSize; i++)
        {
            _boardSquares[i] = new Vector3[_boardSize];
        }
        
        float boardWidth = Math.Abs(corner1.X - corner2.X);
        _squareWidth = boardWidth /  _boardSize; 
        _squareOffset = _squareWidth / 2;
        _minX = Math.Min(corner1.X, corner2.X);
        _minY = Math.Min(corner1.Y, corner2.Y);
        _z = corner1.Z;
        
        PopulateBoardSquarePositions();
    }
    
    public Vector3 GetSquareCenter(int x, int y)
    {
        if (x < 1 || x > _boardSize || y < 1 || y > _boardSize)
        {
            throw new ArgumentException("Invalid board square");
        }
        
        // Subtract 1 from each value to convert from 1-based position to 0-based index
        return _boardSquares[x - 1][y - 1];
    }

    private void PopulateBoardSquarePositions()
    {
        for (int i = 0; i < _boardSize; i++)
        {
            for (int j = 0; j < _boardSize; j++)
            {
                _boardSquares[i][j] = CalculateBoardSquarePosition(i, j);
            }
        }
    }

    private Vector3 CalculateBoardSquarePosition(float xIndex, float yIndex)
    {
        float xPosition = _minX + xIndex * _squareWidth + _squareOffset;
        float yPosition = _minY + yIndex * _squareWidth + _squareOffset;
        return new Vector3(xPosition, yPosition, _z);
    }
}