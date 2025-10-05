using EightQueensRobot.RobotModel;
using EightQueensRobot.Utilities;

namespace EightQueensRobot.GameMaster;

public class TrapezoidalMoveTimer(IRobotModel robotModel) : IMoveTimer
{
    private const float RampTime = 0.25f;
    
    public float CalculateMoveTime(JointAngles initialAngles, JointAngles finalAngles)
    {
        int limitingJoint = GetLongestMovingJoint(initialAngles, finalAngles);
        double degrees = Math.Abs(initialAngles.AsArray[limitingJoint - 1] - finalAngles.AsArray[limitingJoint - 1]);
        float jointSpeedDegreesPerSecond = robotModel.GetRotationalSpeed(limitingJoint);
        double jointSpeedRadiansPerSecond = jointSpeedDegreesPerSecond.ToRadians();
        return CalculateMoveTime((float)degrees, (float)jointSpeedRadiansPerSecond);
    }

    private int GetLongestMovingJoint(JointAngles initialAngles, JointAngles finalAngles)
    {
        double longestMove = 0f;
        int longestMoveJoint = 1;

        for (int i = 0; i < initialAngles.DegreesOfFreedom; i++)
        {
            double degreeChange = Math.Abs(initialAngles.AsArray[i]  - finalAngles.AsArray[i]);
            // GetRotationalSpeed takes 1-based joint number
            float speed = robotModel.GetRotationalSpeed(i + 1);
            double minMoveTime = degreeChange / speed;
            if (minMoveTime > longestMove)
            {
                longestMove = minMoveTime;
            }
        }
        
        return longestMoveJoint;
    }

    private float CalculateMoveTime(float degrees, float speed)
    {
        float rampUpDistance = 0.5f * speed * RampTime;
        float rampDownDistance = 0.5f * speed * RampTime;
        float totalRampDistance = rampUpDistance + rampDownDistance;

        if (degrees <= totalRampDistance)
        {
            return MathF.Sqrt(2f * degrees * RampTime / speed);
        }
        
        float constantSpeedDistance = degrees - totalRampDistance;
        float constantSpeedTime = constantSpeedDistance / speed;

        return RampTime + constantSpeedTime + RampTime;
    }
}