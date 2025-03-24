using System.Collections.Generic;
using TeoEngine.Private;

namespace TeoEngine
{
    public sealed class CustomObject : GameObject
    {
        public List<Vector2> Points = [];
        private readonly List<Vector2> DoublePoints = [];
        private readonly List<IntVector2> IntPoints = [];

        public CustomObject()
        {
            Points.Add(new(-10, -10));
            Points.Add(new(10, -10));
            Points.Add(new(10, 10));
            Points.Add(new(-10, 10));
            OnCreate();
        }

        public CustomObject(Vector2 position, List<Vector2> points, char character)
        {
            Transform.Position = position;
            Points = points;
            View.character = character;
            OnCreate();
        }


        internal override void DrawOnBuffer(ObjectSpace where)
        {
            if (Enabled && View.character != Game.transparentCharacter)
            {
                IntPoints.Clear();
                DoublePoints.Clear();
                for (int i = 0; i < Points.Count; i++)
                {
                    DoublePoints.Add(Vector2.Zero);
                    IntPoints.Add(IntVector2.Zero);
                    DoublePoints[i] = Geometrics.RotatePoint(Points[i], Vector2.Zero, Transform.GlobalAngle);
                    IntPoints[i] = Geometrics.RotatePoint(Points[i], Vector2.Zero, Transform.GlobalAngle).ToInt();
                }
                Draw.DrawPoligon([.. DoublePoints], Transform.GlobalPosition + Game.ScreenSize / 2, this.View);
            }
        }
    }
}
