using System.Numerics;

namespace EightQueensRobot.FKSolver;

// Uses Standard DH Parameters
public sealed class DhLink(
    JointType jointType,                        
    double a,                                   // link length along x_i
    double alpha,                               // link twist about x_i
    double d,                                   // link offset along z_{i-1} (base value)
    double theta,                               // joint angle about z_{i-1} (base value)
    double offset = 0,                          // constant joint offset (radians or meters)
    double min = double.NegativeInfinity,       // joint lower limit (rad or m)
    double max = double.PositiveInfinity        // joint upper limit (rad or m)
    )
{   

    public Matrix4x4 AsMatrix(double q)
    {
        q = Math.Clamp(q, min, max);

        double thetaEffective = theta;
        double dEffective = d;

        if (jointType == JointType.Revolute)
        {
            thetaEffective += (offset + q);
        }
        else
        {
            dEffective += (offset + q);
        }
     
        float ct = (float)Math.Cos(thetaEffective);
        float st = (float)Math.Sin(thetaEffective);
        float ca = (float)Math.Cos(alpha); 
        float sa = (float)Math.Sin(alpha);
        float af = (float)a;
        float df = (float)dEffective;

        return new Matrix4x4(
            ct,         -st,        0f,         af,
            st*ct,      ct*ca,      -sa,        -df*sa,
            st*sa,      ct*sa,      ca,         df*ca,
            0f,         0f,         0f,         1f
        );
    }
}