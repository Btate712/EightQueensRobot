namespace EightQueensRobot.RobotModel;

public record JointAngles(double[] AsArray)
{
    public int DegreesOfFreedom => AsArray.Length;

    public double GetJoint(int angleNumber)
    {
        if (angleNumber < 0 || angleNumber > DegreesOfFreedom)
        {
            throw new IndexOutOfRangeException("The angle number is one-based.");
        }
        
        return AsArray[angleNumber - 1];
    }
}