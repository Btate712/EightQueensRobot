using System.Numerics;
using EightQueensRobot.Reporting;

namespace EightQueensRobot.GameMaster;

public interface IMoveController
{
    void Move(MoveData moveData);
    void Grab();
    void Release();
}