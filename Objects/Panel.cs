using TeoEngine.Private;

namespace TeoEngine
{
    public class Panel : UIElement
    {
        public int roundCorner = 2;

        public Panel()
        {
            OnCreate();
        }
        public Panel(Vector2 position, Vector2 size, char character)
        {
            Transform.Position = position;
            Transform.Size = size;
            View.character = character;
            OnCreate();
        }

        internal override void DrawOnBuffer(ObjectSpace where)
        {
            if (Enabled && where == Location && View.character != Game.transparentCharacter)
            {
                int x, y, i, j;
                x = (int)(Transform.GlobalPosition.X + Game.ScreenSize.X / 2 - Transform.Size.X / 2);
                y = (int)(Transform.GlobalPosition.Y + Game.ScreenSize.Y / 2 - Transform.Size.Y / 2);
                if (Location == ObjectSpace.World)
                {
                    x -= (int)Game.Camera.Transform.Position.X;
                    y -= (int)Game.Camera.Transform.Position.Y;
                }
                for (i = 0; i < Transform.Size.X; i++)
                    for (j = 0; j < Transform.Size.Y; j++)
                        Draw.DrawCharactert(View.character, i + x, j + y, View);
            }
        }
    }
}
