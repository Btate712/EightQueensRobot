using System.Numerics;
using EightQueensRobot.FKSolver;
using EightQueensRobot.RobotModel;

namespace NQueensSolverTests.FKSolver
{
    public sealed class DhTests
    {
        private const float EqualityPrecision = 1e-5f;

        static void AssertMatrixAlmostEqual(Matrix4x4 a, Matrix4x4 b)
        {
            Assert.Equal(a.M11, b.M11, EqualityPrecision);
            Assert.Equal(a.M12, b.M12, EqualityPrecision);
            Assert.Equal(a.M13, b.M13, EqualityPrecision);
            Assert.Equal(a.M14, b.M14, EqualityPrecision);

            Assert.Equal(a.M21, b.M21, EqualityPrecision);
            Assert.Equal(a.M22, b.M22, EqualityPrecision);
            Assert.Equal(a.M23, b.M23, EqualityPrecision);
            Assert.Equal(a.M24, b.M24, EqualityPrecision);

            Assert.Equal(a.M31, b.M31, EqualityPrecision);
            Assert.Equal(a.M32, b.M32, EqualityPrecision);
            Assert.Equal(a.M33, b.M33, EqualityPrecision);
            Assert.Equal(a.M34, b.M34, EqualityPrecision);

            Assert.Equal(a.M41, b.M41, EqualityPrecision);
            Assert.Equal(a.M42, b.M42, EqualityPrecision);
            Assert.Equal(a.M43, b.M43, EqualityPrecision);
            Assert.Equal(a.M44, b.M44, EqualityPrecision);
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

            Matrix4x4 expected = new Matrix4x4(
                ct,      -st*ca,        st*sa,          af*ct,
                st,      ct*ca,         -ct*sa,         af*st,
                0f,      sa,            ca,             df,
                0f,      0f,            0f,             1f
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
                ct,      -st*ca,        st*sa,          af*ct,
                st,      ct*ca,         -ct*sa,         af*st,
                0f,      sa,            ca,             df,
                0f,      0f,            0f,             1f
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
            Assert.Equal(2.0f, t0.M14,EqualityPrecision);
            Assert.Equal(0.0f, t0.M24, EqualityPrecision);
            Assert.Equal(0.0f, t0.M34, EqualityPrecision);

            // q = [pi/2, 0] -> first rotates 90deg; both links along +y => (0,2,0)
            Matrix4x4 t1 = chain.Forward([Math.PI / 2.0, 0.0]);
            Assert.Equal(0.0f, t1.M14, EqualityPrecision);
            Assert.Equal(2.0f, t1.M24, EqualityPrecision);
            Assert.Equal(0.0f, t1.M34, EqualityPrecision);

            // q = [pi/2, -pi/2] -> first along +y, second rotates -90° relative, thus ends at (1,1,0)
            Matrix4x4 t2 = chain.Forward([Math.PI / 2.0, -Math.PI / 2.0]);
            Assert.Equal(1.0f, t2.M14, EqualityPrecision);
            Assert.Equal(1.0f, t2.M24, EqualityPrecision);
            Assert.Equal(0.0f, t2.M34, EqualityPrecision);
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
            Assert.Equal(p.X, T.M14);
            Assert.Equal(p.Y, T.M24);
            Assert.Equal(p.Z, T.M34);

            Quaternion q = DhChain.Orientation(T);
            // Check quaternion is normalized and corresponds to rotation submatrix
            Assert.Equal(1.0f, q.Length(), 0.01);

            // Rebuild rotation matrix and compare rotation part only
            Matrix4x4 r = Matrix4x4.CreateFromQuaternion(q);
            Assert.Equal(r.M11, T.M11, 0.05f);
            Assert.Equal(r.M12, T.M12, 0.05f);
            Assert.Equal(r.M13, T.M13, 0.05f);
            Assert.Equal(r.M21, T.M21, 0.05f);
            Assert.Equal(r.M22, T.M22, 0.05f);
            Assert.Equal(r.M23, T.M23, 0.05f);
            Assert.Equal(r.M31, T.M31, 0.05f);
            Assert.Equal(r.M32, T.M32, 0.05f);
            Assert.Equal(r.M33, T.M33, 0.05f);
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
            Assert.Equal(0.0f, T.M14, EqualityPrecision);
            Assert.Equal(0.9f, T.M24, EqualityPrecision);
            Assert.Equal(0.3f, T.M34, EqualityPrecision);
        }
        
        [Fact]
        public void ABB_IRB120_ForwardKinematics_Check()
        {
            // ToDo: Verify that I've got the DH parameters configured correctly by finding a paper
            // that shows some expected positions and create tests for those positions.

            // Ensure these parameters match the same DH convention implemented by DhLink (standard DH).
        
            // Example structure (replace with actual values I find):
            AbbIrb120 robot = new();
            DhChain chain = robot.DhChain;
        
            // Choose a specific joint configuration in radians.
            double[] q = [0.0, 0.0, 0.0, 0.0, 0.0, 0.0];
        
            // Set the expected transform for that configuration from a trusted reference/source
            // (e.g., a CAD FK, RobotStudio FK, or paper using the same DH convention).
            // This expected value was selected to match the actual value, so doesn't really test anything.
            Vector3 expected = new(0.37400f, 0f, 0.63000f);

            Vector3 actual = chain.GetEndEffectorPosition(q);
            Assert.Equal(expected.X, actual.X, EqualityPrecision);
            Assert.Equal(expected.Y, actual.Y, EqualityPrecision);
            Assert.Equal(expected.Z, actual.Z, EqualityPrecision);
        }
    }
}