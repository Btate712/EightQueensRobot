using System.Numerics;

namespace EightQueensRobot.Reporting;

public record MoveReportingData(QueenMove QueenMove, Vector3 TargetPosition, Vector3 ActualPosition, float MoveTime);