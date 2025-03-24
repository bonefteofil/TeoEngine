using System;
using TeoEngine.Private;

namespace TeoEngine
{
    public sealed class Circle : GameObject
    {
        public Circle()
        {
            OnCreate();
        }

        public Circle(Vector2 position, Vector2 size, char character)
        {
            Transform.Position = position;
            Transform.Size = size;
            View.character = character;
            OnCreate();
        }


        internal override void DrawOnBuffer(ObjectSpace where)
        {
            if (Enabled && View.character != Game.transparentCharacter && Transform.Size.X != 0 && Transform.Size.Y != 0)
            {
                int x = (int)(Transform.GlobalPosition.X + Game.ScreenSize.X / 2 - Game.Camera.Transform.Position.X);
                int y = (int)(Transform.GlobalPosition.Y + Game.ScreenSize.Y / 2 - Game.Camera.Transform.Position.Y);
                int sizex = (int)Math.Max(Math.Abs(Transform.Size.X), 10);
                int sizey = (int)Math.Max(Math.Abs(Transform.Size.Y), 10);

                for (int i = -sizex; i < sizex; i++)
                    for (int j = -sizey; j < sizey; j++)
                        if (!((double)(i * i / (double)(Transform.Size.X * Transform.Size.X)) + (double)(j * j / (double)(Transform.Size.Y * Transform.Size.Y)) + 0.6 > 1))
                            Draw.DrawCharactert(View.character, i + x, j + y, View);
            }
        }
    }
}
