using System;
using TeoEngine.Private;

namespace TeoEngine
{
    public class Button : UIElement
    {
        public ConsoleKey buttonKey;
        public bool clicked = false;
        public bool mouseOver = false;
        public Action<Element> OnClickDown;
        public Action<Element> OnClickUp;
        public Action<Element> OnOver;
        public Action<Element> OnLeave;

        public Button()
        {
            OnCreate();
        }
        public Button(Vector2 position, Vector2 size, char character)
        {
            Transform.Position = position;
            Transform.Size = size;
            View.character = character;
            OnCreate();
        }

        internal override void DrawOnBuffer(ObjectSpace where)
        {
            if (!Enabled)
            {
                if (clicked)
                {
                    clicked = false;
                    OnClickUp?.Invoke(this);
                }
                if (mouseOver)
                {
                    mouseOver = false;
                    OnLeave?.Invoke(this);
                }
            }
            else if (where == Location)
            {
                if (OnClickDown != null && Input.GetKeyDown(buttonKey))  //  key press
                    OnClickDown.Invoke(this);
                if (OnClickUp != null && Input.GetKeyUp(buttonKey))
                    OnClickUp.Invoke(this);

                if (Input.MousePosition.ToInt() >= (Transform.GlobalPosition - Transform.Size / 2).ToInt() && Input.MousePosition.ToInt() < (Transform.GlobalPosition + Transform.Size / 2).ToInt())  // mouse over
                {
                    if (!mouseOver && OnOver != null)
                        OnOver.Invoke(this);
                    mouseOver = true;

                    if (Input.GetMouseButtonDown(0))  //  mouse down
                    {
                        clicked = true;
                        OnClickDown?.Invoke(this);
                    }
                }
                else  //  mouse leave
                {
                    if (mouseOver && OnLeave != null)
                        OnLeave.Invoke(this);
                    mouseOver = false;
                }
                if (Input.GetMouseButtonUp(0) && clicked)  //  mouse up
                {
                    clicked = false;
                    OnClickUp?.Invoke(this);
                }


                if (View.character != Game.transparentCharacter)
                {
                    int x, y, i, j;
                    x = (int)Math.Round(Transform.GlobalPosition.X + Game.ScreenSize.X / 2 - Transform.Size.X / 2);
                    y = (int)Math.Round(Transform.GlobalPosition.Y + Game.ScreenSize.Y / 2 - Transform.Size.Y / 2);
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
}
