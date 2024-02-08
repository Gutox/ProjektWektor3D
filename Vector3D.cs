using System;
using System.Text.Json.Serialization;

namespace ProjektWektor3D
{
    public class Vector3D
    {
        // Constructors
        [JsonConstructor]
        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3D(decimal x, decimal y, decimal z)
        {
            X = decimal.ToDouble(x);
            Y = decimal.ToDouble(y);
            Z = decimal.ToDouble(z);
        }


        public Vector3D(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double this[int i]
        {
            get
            {
                if (i == 0) return X;

                if (i == 1) return Y;

                if (i == 2) return Z;

                throw new IndexOutOfRangeException();
            }

            set
            {
                if (i == 0) X = value;
                else if (i == 1) Y = value;
                else if (i == 2) Z = value;
                else throw new IndexOutOfRangeException();
            }
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public static bool operator ==(Vector3D a, Vector3D b)
        {
            return Math.Abs(a.X - b.X) < 0.01 && Math.Abs(a.Y - b.Y) < 0.01 &&
                   Math.Abs(a.Z - b.Z) < 0.01;
        }

        public static bool operator !=(Vector3D a, Vector3D b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3D)
            {
                var other = (Vector3D)obj;
                return this == other;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        // Operators and functions      

        public static Vector3D operator -(Vector3D a)
        {
            return new Vector3D(-a.X, -a.Y, -a.Z);
        }

        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            return new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3D operator *(Vector3D a, double scalar)
        {
            return new Vector3D(a.X * scalar, a.Y * scalar, a.Z * scalar);
        }

        public static Vector3D operator /(Vector3D a, double scalar)
        {
            return new Vector3D(a.X / scalar, a.Y / scalar, a.Z / scalar);
        }

        public double DotProduct(Vector3D other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        public Vector3D CrossProduct(Vector3D other)
        {
            return new Vector3D(Y * other.Z - Z * other.Y, Z * other.X - X * other.Z, X * other.Y - Y * other.X);
        }

        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public double Length()
        {
            return Magnitude();
        }

        public Vector3D Normalize()
        {
            return this / Magnitude();
        }

        public double Angle(Vector3D other)
        {
            return Math.Acos(DotProduct(other) / (Magnitude() * other.Magnitude()));
        }

        public double Distance(Vector3D other)
        {
            return (this - other).Length();
        }
    }
}