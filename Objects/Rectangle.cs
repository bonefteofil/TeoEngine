using TeoEngine.Private;

namespace TeoEngine
{
    public sealed class Rectangle : GameObject
    {
        private readonly Vector2[] Points = new Vector2[4];
        private readonly IntVector2[] pPoints = new IntVector2[4];

        public Rectangle()
        {
            OnCreate();
        }

        public Rectangle(Vector2 position, Vector2 size, char character)
        {
            Transform.Position = position;
            Transform.Size = size;
            View.character = character;
            OnCreate();
        }


        internal override void DrawOnBuffer(ObjectSpace where)
        {
            if (Enabled && View.character != Game.transparentCharacter)
            {
                Points[0] = Vector2.Zero + Transform.Size / -2;
                Points[1] = Vector2.Zero + new Vector2(Transform.Size.X / 2, Transform.Size.Y / -2);
                Points[2] = Vector2.Zero + Transform.Size / 2;
                Points[3] = Vector2.Zero + new Vector2(Transform.Size.X / -2, Transform.Size.Y / 2);
                for (int i = 0; i < 4; i++)
                {
                    Points[i] = Geometrics.RotatePoint(Points[i], Vector2.Zero, Transform.GlobalAngle);
                    Points[i] = Geometrics.RotatePoint(Points[i], Game.Camera.Transform.Position - Transform.GlobalPosition, Game.Camera.Transform.Angle);
                    pPoints[i] = Points[i].ToInt();
                }
                Draw.DrawPoligon(Points, Transform.GlobalPosition + Game.ScreenSize / 2 - Game.Camera.Transform.Position, this.View);
            }
        }
    }
}
