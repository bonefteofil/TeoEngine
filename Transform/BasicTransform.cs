using TeoEngine.Private;

namespace TeoEngine.Transform
{
    public class BasicTransform
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 GlobalPosition { get; internal set; } = Vector2.Zero;
        public Vector2 Size { get; set; } = Vector2.One;
        private int pZIndex = 0;
        public int ZIndex { get => pZIndex; set { pZIndex = value; Application.SortObjects(); Application.SortGameObjects(); Application.SortUIElements(); } }


        public void Move(double x, double y)
        {
            Position += new Vector2(x / 10, y / 10);
        }

        public void Resize(double x, double y)
        {
            Size += new Vector2(x, y);
        }
    }
}
