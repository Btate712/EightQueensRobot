namespace EightQueensRobot.RobotModel;

public record SixDofJointData(float Joint1, float Joint2, float Joint3, float Joint4, float Joint5, float Joint6)
{
    public double[] AsArray()
    {
        return
        [
            Joint1,
            Joint2,
            Joint3,
            Joint4,
            Joint5,
            Joint6
        ];
    }
}