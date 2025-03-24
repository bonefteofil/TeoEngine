using System;

namespace TeoEngine
{
    public class Vector2
    {
        public double X { get; } = 0;
        public double Y { get; } = 0;

        public static Vector2 Zero => new(0, 0);
        public static Vector2 One => new(1, 1);

        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);

        public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);

        public static Vector2 operator *(Vector2 a, Vector2 b) => new(a.X * b.X, a.Y * b.Y);

        public static Vector2 operator *(Vector2 a, int b) => new(a.X * b, a.Y * b);

        public static Vector2 operator /(Vector2 a, int b) => new(a.X / b, a.Y / b);

        public static bool operator ==(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Vector2 a, Vector2 b) => a.X != b.X || a.Y != b.Y;

        public static bool operator >=(Vector2 a, Vector2 b) => a.X >= b.X && a.Y >= b.Y;

        public static bool operator <=(Vector2 a, Vector2 b) => a.X <= b.X && a.Y <= b.Y;

        public static bool operator >(Vector2 a, Vector2 b) => a.X > b.X && a.Y > b.Y;

        public static bool operator <(Vector2 a, Vector2 b) => a.X < b.X && a.Y < b.Y;

        public override string ToString() => $"{X}, {Y}".ToString();

        public override bool Equals(object obj) => obj is Vector2 vector && this == vector;

        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}