namespace EightQueensRobot.Reporting;

public interface IDataOutput
{
    void AddData(MoveReportingData moveReportingData);
    void AddData(string data);
    void WriteData();
}