namespace EightQueensRobot.Reporting;

public class ConsoleWriter : IDataOutput
{
    private readonly List<MoveReportingData> _moveData = [];
    private List<string> _dataOutput = [];
    
    public void AddData(MoveReportingData moveReportingData)
    {
        _moveData.Add(moveReportingData);
    }

    public void AddData(string data)
    {
        _dataOutput.Add(data);
    }
    
    public void WriteData()
    {
        QueenMove? lastMove = null;
        
        foreach (var datum in _moveData)
        {
            if (datum.QueenMove != lastMove)
            {
                Console.WriteLine($"Move queen from {datum.QueenMove.Source} to {datum.QueenMove.Destination}:");
                lastMove = datum.QueenMove;
            }
            
            Console.WriteLine($"\tTarget: {datum.TargetPosition}\tActual: {datum.ActualPosition}\tTime: {datum.MoveTime}");
        }
    }
}