using System.Numerics;
using EightQueensRobot.FKSolver;
using EightQueensRobot.IKSolver;
using EightQueensRobot.Reporting;
using EightQueensRobot.RobotModel;

namespace EightQueensRobot.GameMaster;

public class DefaultMoveController: IMoveController
{
    private const float VerticalClearance = 10f;
    
    private readonly IMoveTimer _moveTimer;
    private readonly IDataOutput _dataOutput;
    private readonly IIkSolver<JointAngles> _ikSolver;
    private readonly DhChain _fkSolver;
    
    private Vector3 _currentPosition;
    private JointAngles _currentJointPositions;

    public DefaultMoveController(IMoveTimer moveTimer, IDataOutput dataOutput, IIkSolver<JointAngles> ikSolver, IRobotModel robotModel, Vector3 startPosition)
    {
        _currentPosition = startPosition;
        _moveTimer = moveTimer;
        _dataOutput = dataOutput;
        _ikSolver = ikSolver;
        _currentJointPositions = _ikSolver.GetJointAnglesForPosition(_currentPosition);
        _fkSolver = robotModel.DhChain;
    }
    
    public void Move(MoveData moveData)
    {
        MoveStraightUp(moveData);
        MoveAboveTargetPosition(moveData);
        MoveToTargetPosition(moveData);
    }

    public void Grab()
    {
        _dataOutput.AddData("Grab");
    }

    public void Release()
    {
        _dataOutput.AddData("Release");
    }

    public Vector3 CurrentPosition => _currentPosition;
    
    private void MoveStraightUp(MoveData moveData)
    {
        Vector3 straightUpPosition = _currentPosition with { Z = _currentPosition.Z + VerticalClearance };
        MovePartial(straightUpPosition, moveData);
    }

    private void MoveAboveTargetPosition(MoveData moveData)
    {
        Vector3 aboveTargetPosition = moveData.TargetPosition with { Z = _currentPosition.Z + VerticalClearance };
        MovePartial(aboveTargetPosition, moveData);
    }

    private void MoveToTargetPosition(MoveData moveData)
    {
        MovePartial(moveData.TargetPosition, moveData);
    }
    
    private void MovePartial(Vector3 targetPosition, MoveData overallMoveData)
    {
        JointAngles targetJointAngles = _ikSolver.GetJointAnglesForPosition(targetPosition);
        
        float moveTime = _moveTimer.CalculateMoveTime(_currentJointPositions, targetJointAngles);
        Vector3 actualPositionForJointAngles = _fkSolver.GetEndEffectorPosition(targetJointAngles.AsArray);
        
        MoveReportingData moveReportingData = new(overallMoveData.QueenMove, targetPosition, actualPositionForJointAngles, moveTime);
        
        _dataOutput.AddData(moveReportingData);
        
        _currentPosition = actualPositionForJointAngles;
        _currentJointPositions = targetJointAngles;
    }
}