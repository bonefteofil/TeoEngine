using System;
using TeoEngine.Private;

namespace TeoEngine
{
    public class Text : UIElement
    {
        public string text = "New text!";
        public HorizontalAlign align = HorizontalAlign.Center;

        public Text()
        {
            OnCreate();
        }
        public Text(Vector2 position, string text)
        {
            this.text = text;
            Transform.Position = position;
            OnCreate();
        }

        internal override void DrawOnBuffer(ObjectSpace where)
        {
            if (Enabled && where == Location)
            {
                int x, y;
                x = (int)(Transform.GlobalPosition.X + Game.ScreenSize.X / 2);
                y = (int)(Transform.GlobalPosition.Y + Game.ScreenSize.Y / 2);
                if (Location == ObjectSpace.World)
                {
                    x -= (int)Game.Camera.Transform.Position.X;
                    y -= (int)Game.Camera.Transform.Position.Y;
                }
                if (align == HorizontalAlign.Center)
                    x -= text.Length / 2;
                else if (align == HorizontalAlign.Right)
                    x -= text.Length;

                for (int i = 0; i < text.Length; i++)
                    Draw.DrawCharactert(text[i], i + x, y, View);
            }
        }
    }
}
