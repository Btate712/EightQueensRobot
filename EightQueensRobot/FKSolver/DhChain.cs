using System.Numerics;

namespace EightQueensRobot.FKSolver;

public sealed class DhChain
{
    public readonly List<DhLink> Links = [];

    public DhChain(IEnumerable<DhLink> links) => Links.AddRange(links);

    /// Compute forward kinematics. Returns the end-effector transform relative to base.
    public Matrix4x4 Forward(double[] q)
    {
        if (q.Length != Links.Count) throw new ArgumentException("q length must match link count");
        Matrix4x4 T = Matrix4x4.Identity;
        for (int i = 0; i < Links.Count; i++)
            T = Matrix4x4.Multiply(Links[i].AsMatrix(q[i]), T);
        return T;
    }

    /// Optionally return per-joint intermediate frames (useful for Jacobians, viz, debugging)
    public Matrix4x4[] ForwardAll(double[] q)
    {
        if (q.Length != Links.Count) throw new ArgumentException("q length must match link count");
        var outFrames = new Matrix4x4[Links.Count + 1];
        Matrix4x4 T = Matrix4x4.Identity;
        outFrames[0] = T;
        for (int i = 0; i < Links.Count; i++)
        {
            T = Matrix4x4.Multiply(Links[i].AsMatrix(q[i]), T);
            outFrames[i + 1] = T;
        }

        return outFrames;
    }

    public Vector3 GetEndEffectorPosition(double[] q)
    {
        Matrix4x4 T = Forward(q);
        return Position(T);
    }
    
    public static Vector3 Position(Matrix4x4 T) => new(T.M41, T.M42, T.M43);
    public static Quaternion Orientation(Matrix4x4 T) => Quaternion.CreateFromRotationMatrix(T);
}