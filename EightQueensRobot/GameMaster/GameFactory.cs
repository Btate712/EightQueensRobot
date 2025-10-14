using System.Numerics;
using EightQueensRobot.IKSolver;
using EightQueensRobot.PuzzleSolver;
using EightQueensRobot.Reporting;
using EightQueensRobot.RobotModel;
using EightQueensRobot.Utilities;

namespace EightQueensRobot.GameMaster;

public class GameFactory
{
    const int NumberOfQueens = 8;
    private const float BoardWidth = 0.24f; // 24 cm x 24 cm
    
    public GameManager GetDefaultGame()
    {
        Vector3 boardCorner1 = new(0f, 0f, 0f);
        Vector3 boardCorner2 = new(BoardWidth, BoardWidth, 0f);
        Vector3 hopperPosition = new(-0.25f, -0.25f, -0.25f);
        const int numberOfIterations = 1000;
        const float tolerance = 0.001f;
        const int divisionsPerDimension = 8;
        
        NQueensSolver eightQueensSolver = new NQueensSolver(NumberOfQueens);
        BoardManager boardManager = new BoardManager(NumberOfQueens, boardCorner1, boardCorner2);
        Sungur370 robotModel = new();
        TrapezoidalMoveTimer moveTimer = new(robotModel);
        TextFileWriter writer = new();
        RandomNumberGenerator randomNumberGenerator = new();
        DefaultFireflyAttractionHeuristic heuristic = new(randomNumberGenerator, robotModel);
        FireflyCache nullFireflyCache = new(robotModel, divisionsPerDimension);
        DefaultFireflySwarmHandler swarmHandler = new(robotModel, heuristic, randomNumberGenerator, nullFireflyCache);
        WithinPositionToleranceExitCriteriaHandler exitCriteriaHandler = new(swarmHandler, tolerance, numberOfIterations);
        FireflyIkSolver ikSolver = new(exitCriteriaHandler, robotModel, swarmHandler);
        DefaultMoveController moveController = new(moveTimer, writer, ikSolver, robotModel, hopperPosition);
        QueenPositionManager queenPositionManager = new(NumberOfQueens);
        GameManager gameManager = new(eightQueensSolver, boardManager, moveController, queenPositionManager, writer);
        return gameManager;
    }
}