using System.Numerics;
using EightQueensRobot.FKSolver;
using EightQueensRobot.RobotModel;

namespace NQueensSolverTests.FKSolver
{
    public sealed class DhTests
    {
        private const float AlmostEqualPrecision = 1e-5f;

        static void AssertAlmostEqual(float a, float b, float eps = AlmostEqualPrecision)
        {
            Assert.True(MathF.Abs(a - b) <= eps, $"Expected {b}, got {a}");
        }

        static void AssertVectorAlmostEqual(Vector3 a, Vector3 b, float eps = AlmostEqualPrecision)
        {
            AssertAlmostEqual(a.X, b.X, eps);
            AssertAlmostEqual(a.Y, b.Y, eps);
            AssertAlmostEqual(a.Z, b.Z, eps);
        }

        static void AssertMatrixAlmostEqual(Matrix4x4 a, Matrix4x4 b, float eps = AlmostEqualPrecision)
        {
            AssertAlmostEqual(a.M11, b.M11, eps);
            AssertAlmostEqual(a.M12, b.M12, eps);
            AssertAlmostEqual(a.M13, b.M13, eps);
            AssertAlmostEqual(a.M14, b.M14, eps);

            AssertAlmostEqual(a.M21, b.M21, eps);
            AssertAlmostEqual(a.M22, b.M22, eps);
            AssertAlmostEqual(a.M23, b.M23, eps);
            AssertAlmostEqual(a.M24, b.M24, eps);

            AssertAlmostEqual(a.M31, b.M31, eps);
            AssertAlmostEqual(a.M32, b.M32, eps);
            AssertAlmostEqual(a.M33, b.M33, eps);
            AssertAlmostEqual(a.M34, b.M34, eps);

            AssertAlmostEqual(a.M41, b.M41, eps);
            AssertAlmostEqual(a.M42, b.M42, eps);
            AssertAlmostEqual(a.M43, b.M43, eps);
            AssertAlmostEqual(a.M44, b.M44, eps);
        }

        [Fact]
        public void SingleRevolute_AsMatrix_MatchesStandardDH()
        {
            // Parameters (radians, meters)
            const double a = 0.5;
            const double alpha = Math.PI / 4.0; // 45 deg
            const double d = 0.2;
            const double thetaBase = Math.PI / 6.0; // 30 deg
            const double offset = Math.PI / 12.0; // 15 deg

            DhLink link = new(
                jointType: JointType.Revolute,
                a: a,
                alpha: alpha,
                d: d,
                theta: thetaBase,
                offset: offset
            );

            const double q = 0.1; // rad
            const double thetaEff = thetaBase + (offset + q);
            float ct = (float)Math.Cos(thetaEff);
            float st = (float)Math.Sin(thetaEff);
            float ca = (float)Math.Cos(alpha);
            float sa = (float)Math.Sin(alpha);
            const float af = (float)a;
            const float df = (float)d;

            Matrix4x4 expected = new(
                ct,    st*ca,   st*sa,    0f,
                -st,   ct*ca,   ct * sa,  0f,
                0f,    -sa,     ca,       0f,
                af*ct, af*st,   df,       1f
            );

            Matrix4x4 actual = link.AsMatrix(q);
            AssertMatrixAlmostEqual(actual, expected);
        }

        [Fact]
        public void SinglePrismatic_AsMatrix_MatchesStandardDH()
        {
            // Prismatic: dEffective = D + (Offset + q), theta stays Theta
            const double a = 0.3;
            const double alpha = -Math.PI / 2.0;
            const double dBase = 0.1;
            const double thetaBase = -Math.PI / 3.0;
            const double offset = 0.05;

            DhLink link = new(
                jointType: JointType.Prismatic,
                a: a,
                alpha: alpha,
                d: dBase,
                theta: thetaBase,
                offset: offset
            );

            const double q = 0.2; // meters
            const double dEff = dBase + (offset + q);

            float ct = (float)Math.Cos(thetaBase);
            float st = (float)Math.Sin(thetaBase);
            float ca = (float)Math.Cos(alpha);
            float sa = (float)Math.Sin(alpha);
            const float af = (float)a;
            const float df = (float)dEff;

            Matrix4x4 expected = new(
                ct,    st*ca,   st*sa,    0f,
                -st,   ct*ca,   ct * sa,  0f,
                0f,    -sa,     ca,       0f,
                af*ct, af*st,   df,       1f
            );

            Matrix4x4 actual = link.AsMatrix(q);
            AssertMatrixAlmostEqual(actual, expected);
        }

        [Fact]
        public void JointLimits_AreClamped()
        {
            DhLink link = new(
                jointType: JointType.Revolute,
                a: 0.0,
                alpha: 0.0,
                d: 0.0,
                theta: 0.0,
                offset: 0.0,
                min: -0.5,
                max: 0.5
            );

            // Exceeds max => should clamp to 0.5
            Matrix4x4 high = link.AsMatrix(2.0);
            Matrix4x4 max = link.AsMatrix(0.5);
            AssertMatrixAlmostEqual(high, max);

            // Exceeds min => should clamp to -0.5
            Matrix4x4 low = link.AsMatrix(-2.0);
            Matrix4x4 min = link.AsMatrix(-0.5);
            AssertMatrixAlmostEqual(low, min);
        }

        [Fact]
        public void Chain_Composes_InCorrectOrder()
        {
            // Two planar revolute links with alpha=0, d=0
            // Link1: a1=1, theta base=0
            // Link2: a2=1, theta base=0
            DhLink l1 = new(JointType.Revolute, a: 1.0, alpha: 0, d: 0, theta: 0);
            DhLink l2 = new(JointType.Revolute, a: 1.0, alpha: 0, d: 0, theta: 0);
            DhChain chain = new([l1, l2]);

            // q = [0, 0] -> position (2,0,0)
            Matrix4x4 t0 = chain.Forward([0.0, 0.0]);
            AssertAlmostEqual(t0.M41, 2.0f);
            AssertAlmostEqual(t0.M42, 0.0f);
            AssertAlmostEqual(t0.M43, 0.0f);

            // q = [pi/2, 0] -> first rotates 90deg; both links along +y => (0,2,0)
            Matrix4x4 t1 = chain.Forward([Math.PI / 2.0, 0.0]);
            AssertAlmostEqual(t1.M41, 0.0f);
            AssertAlmostEqual(t1.M42, 2.0f);
            AssertAlmostEqual(t1.M43, 0.0f);

            // q = [pi/2, -pi/2] -> first along +y, second rotates -90° relative, thus ends at (1,1,0)
            Matrix4x4 t2 = chain.Forward([Math.PI / 2.0, -Math.PI / 2.0]);
            AssertAlmostEqual(t2.M41, 1.0f);
            AssertAlmostEqual(t2.M42, 1.0f);
            AssertAlmostEqual(t2.M43, 0.0f);
        }

        [Fact]
        public void ForwardAll_Returns_Base_And_PerJoint_Frames()
        {
            DhLink l1 = new(JointType.Revolute, a: 1.0, alpha: 0, d: 0, theta: 0);
            DhLink l2 = new(JointType.Revolute, a: 1.0, alpha: 0, d: 0, theta: 0);
            DhChain chain = new([l1, l2]);

            Matrix4x4[] frames = chain.ForwardAll([0.0, 0.0]);
            Assert.Equal(3, frames.Length);

            // Base is identity
            AssertMatrixAlmostEqual(frames[0], Matrix4x4.Identity);

            // End-effector equals Forward
            Matrix4x4 T = chain.Forward([0.0, 0.0]);
            AssertMatrixAlmostEqual(frames[^1], T);
        }

        [Fact]
        public void Position_And_Orientation_Extraction_Are_Consistent()
        {
            DhLink l = new(JointType.Revolute, a: 0.4, alpha: Math.PI / 6.0, d: 0.3, theta: Math.PI / 7.0);
            Matrix4x4 T = l.AsMatrix(0.2);

            Vector3 p = DhChain.Position(T);
            AssertAlmostEqual(p.X, T.M41);
            AssertAlmostEqual(p.Y, T.M42);
            AssertAlmostEqual(p.Z, T.M43);

            Quaternion q = DhChain.Orientation(T);
            // Check quaternion is normalized and corresponds to rotation submatrix
            AssertAlmostEqual(q.Length(), 1.0f, 1e-4f);

            // Rebuild rotation matrix and compare rotation part only
            Matrix4x4 r = Matrix4x4.CreateFromQuaternion(q);
            AssertAlmostEqual(r.M11, T.M11);
            AssertAlmostEqual(r.M12, T.M12);
            AssertAlmostEqual(r.M13, T.M13);
            AssertAlmostEqual(r.M21, T.M21);
            AssertAlmostEqual(r.M22, T.M22);
            AssertAlmostEqual(r.M23, T.M23);
            AssertAlmostEqual(r.M31, T.M31);
            AssertAlmostEqual(r.M32, T.M32);
            AssertAlmostEqual(r.M33, T.M33);
        }

        [Fact]
        public void EmptyChain_IsIdentity()
        {
            DhChain chain = new([]);
            Matrix4x4 T = chain.Forward([]);
            AssertMatrixAlmostEqual(T, Matrix4x4.Identity);

            Matrix4x4[] frames = chain.ForwardAll([]);
            Assert.Single(frames);
            AssertMatrixAlmostEqual(frames[0], Matrix4x4.Identity);
        }

        [Fact]
        public void Forward_Throws_On_Q_Length_Mismatch()
        {
            DhLink l1 = new(JointType.Revolute, a: 1.0, alpha: 0, d: 0, theta: 0);
            DhChain chain = new([l1]);

            Assert.Throws<ArgumentException>(() => chain.Forward([]));
            Assert.Throws<ArgumentException>(() => chain.ForwardAll([]));
        }

        [Fact]
        public void Offsets_Are_Applied_Correctly()
        {
            // Revolute offset test: thetaEffective = Theta + (Offset + q)
            DhLink l = new(JointType.Revolute, a: 0.0, alpha: 0.0, d: 0.0, theta: 0.0, offset: 0.25);
            Matrix4x4 tq = l.AsMatrix(0.1);
            Matrix4x4 tEquivalent = new DhLink(JointType.Revolute, a: 0.0, alpha: 0.0, d: 0.0, theta: 0.35).AsMatrix(0.0);
            AssertMatrixAlmostEqual(tq, tEquivalent);

            // Prismatic offset test: dEffective = D + (Offset + q)
            DhLink lp = new(JointType.Prismatic, a: 0.0, alpha: 0.0, d: 0.1, theta: 0.0, offset: 0.05);
            Matrix4x4 tp = lp.AsMatrix(0.2);
            Matrix4x4 tEquivalentP = new DhLink(JointType.Prismatic, a: 0.0, alpha: 0.0, d: 0.35, theta: 0.0).AsMatrix(0.0);
            AssertMatrixAlmostEqual(tp, tEquivalentP);
        }

        [Fact]
        public void Composition_With_Mixed_Revolute_And_Prismatic()
        {
            // Link1 revolute rotates 90deg, link2 prismatic extends along its Z (affects d)
            DhLink l1 = new(JointType.Revolute, a: 0.5, alpha: 0.0, d: 0.0, theta: 0.0);
            DhLink l2 = new(JointType.Prismatic, a: 0.4, alpha: 0.0, d: 0.1, theta: 0.0);
            DhChain chain = new([l1, l2]);

            Matrix4x4 T = chain.Forward([Math.PI / 2.0, 0.2]); // d2 = 0.1 + 0.2
            // Analytical expectation:
            // After l1: rotate 90° about z, translate x by 0.5 -> position (0, 0.5, 0)
            // l2: rotation identity, translate x by 0.4 (but in rotated frame) and z by d2 = 0.3
            // So final position: (0 - because x maps to +y), y = 0.5 + 0.4 = 0.9, z = 0.3
            AssertAlmostEqual(T.M41, 0.0f);
            AssertAlmostEqual(T.M42, 0.9f);
            AssertAlmostEqual(T.M43, 0.3f);
        }
        
        [Fact]
        public void ABB_IRB120_ForwardKinematics_Check()
        {
            // ToDo: Verify that I've got the DH parameters configured correctly by finding a paper
            // that shows some expected positions and create tests for those positions.

            // Ensure these parameters match the same DH convention implemented by DhLink (standard DH).
        
            // Example structure (replace with actual values I find):
            DhChain chain = AbbIrb120.DhChain;
        
            // Choose a specific joint configuration in radians.
            double[] q = [0.0, 0.0, 0.0, 0.0, 0.0, 0.0];
        
            // Set the expected transform for that configuration from a trusted reference/source
            // (e.g., a CAD FK, RobotStudio FK, or paper using the same DH convention).
            // This expected value was selected to match the actual value, so doesn't really test anything.
            Vector3 expected = new(0.37400f, 0, 0.63000f);

            Vector3 actual = chain.GetEndEffectorPosition(q);
            AssertVectorAlmostEqual(expected, actual);
        }
    }
}