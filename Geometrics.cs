using System;

namespace TeoEngine
{
    public static class Geometrics
    {
        public static Vector2 CenterOfPoligon(Vector2[] points)
        {
            Vector2 v = Vector2.Zero;
            for (int i = 0; i < points.Length; i++)
                v += points[i];

            return v / points.Length;
        }

        public static Vector2 RotatePoint(Vector2 point, Vector2 pivot, double angle)
        {
            double DeltaX = point.X - pivot.X;
            double DeltaY = point.Y - pivot.Y;
            double AngleCosine = Math.Cos(ConvertToRadians(angle));
            double AngleSine = Math.Sin(ConvertToRadians(angle));

            return new Vector2(DeltaX * AngleCosine - DeltaY * AngleSine + pivot.X, DeltaX * AngleSine + DeltaY * AngleCosine + pivot.Y);
        }

        public static double ConvertToRadians(double angle) => Math.PI / 180 * angle;

        public static bool InArea(Vector2 poz, Vector2 poz1, Vector2 poz2) => poz >= poz1 && poz <= poz2;

        public static IntVector2 ToInt(this Vector2 a) => new((int)a.X, (int)a.Y);

        public static Vector2 ToDouble(this IntVector2 a) => new(a.X, a.Y);
    }
}
