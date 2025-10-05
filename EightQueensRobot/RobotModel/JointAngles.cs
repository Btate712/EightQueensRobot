namespace EightQueensRobot.RobotModel;

public record JointAngles(double[] AsArray)
{
    public int DegreesOfFreedom => AsArray.Length;

    public double GetJoint(int angleNumber)
    {
        return AsArray[angleNumber];
    }
}