using System;

namespace TeoEngine
{
    public class IntVector2
    {
        public int X { get; } = 0;
        public int Y { get; } = 0;

        public static IntVector2 Zero => new(0, 0);
        public static IntVector2 One => new(1, 1);

        public IntVector2()
        {
            X = 0;
            Y = 0;
        }

        public IntVector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static IntVector2 operator +(IntVector2 a, IntVector2 b) => new(a.X + b.X, a.Y + b.Y);

        public static Vector2 operator +(IntVector2 a, Vector2 b) => new(a.X + (int)b.X, a.Y + (int)b.Y);

        public static Vector2 operator +(Vector2 a, IntVector2 b) => new((int)a.X + b.X, (int)a.Y + b.Y);

        public static IntVector2 operator -(IntVector2 a, IntVector2 b) => new(a.X - b.X, a.Y - b.Y);

        public static Vector2 operator -(IntVector2 a, Vector2 b) => new(a.X - (int)b.X, a.Y - (int)b.Y);

        public static Vector2 operator -(Vector2 a, IntVector2 b) => new(a.X - b.X, a.Y - b.Y);

        public static IntVector2 operator *(IntVector2 a, int b) => new(a.X * b, a.Y * b);

        public static IntVector2 operator /(IntVector2 a, int b) => new(a.X / b, a.Y / b);

        public static bool operator ==(IntVector2 a, IntVector2 b) => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(IntVector2 a, IntVector2 b) => a.X != b.X || a.Y != b.Y;

        public static bool operator >=(IntVector2 a, IntVector2 b) => a.X >= b.X && a.Y >= b.Y;

        public static bool operator <=(IntVector2 a, IntVector2 b) => a.X <= b.X && a.Y <= b.Y;

        public static bool operator >(IntVector2 a, IntVector2 b) => a.X > b.X && a.Y > b.Y;

        public static bool operator <(IntVector2 a, IntVector2 b) => a.X < b.X && a.Y < b.Y;

        public override string ToString() => $"{X}, {Y}".ToString();

        public override bool Equals(object obj) => obj is IntVector2 vector && this == vector;

        public override int GetHashCode() => HashCode.Combine(X, Y);
    }
}
