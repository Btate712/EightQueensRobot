namespace EightQueensRobot.Reporting;

public class TextFileWriter : IDataOutput
{
    private readonly List<MoveReportingData> _data = [];
    private readonly List<string> _dataOutput = [];
    
    public void AddData(MoveReportingData moveReportingData)
    {
        _data.Add(moveReportingData);
    }

    public void AddData(string data)
    {
        _dataOutput.Add(data);
    }

    public void WriteData()
    {
        string fileName = $"MoveReport_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
    
        using (StreamWriter writer = new(fileName))
        {
            QueenMove? lastMove = null;
        
            foreach (MoveReportingData datum in _data)
            {
                if (datum.QueenMove != lastMove)
                {
                    writer.WriteLine($"Move queen from {datum.QueenMove.Source} to {datum.QueenMove.Destination}:");
                    lastMove = datum.QueenMove;
                }
            
                writer.WriteLine($"\tTarget: {datum.TargetPosition}\tActual: {datum.ActualPosition}\tTime: {datum.MoveTime}");
            }
        }
    
        Console.WriteLine($"Data written to {fileName}");
    }
}