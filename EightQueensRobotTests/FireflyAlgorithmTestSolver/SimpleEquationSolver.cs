using System.Numerics;

namespace EightQueensRobot.FKSolver;

public static class SimpleEquationSolver
{
    public static Vector3 Solve(float[] variables)
    {
        float a = variables[0];
        float b = variables[1];
        float c = variables[2];
        float d = variables[3];
        float e = variables[4];
        float f = variables[5];

        float x = a * a - b + c;
        float y = c * c - d + e;
        float z = d * d + e - f;
        
        return new Vector3(x, y, z);
    }
}