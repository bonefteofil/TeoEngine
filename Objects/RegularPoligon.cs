using System;
using System.Collections.Generic;
using TeoEngine.Transform;
using TeoEngine.Private;

namespace TeoEngine
{
    public sealed class RegularPoligon : GameObject
    {
        private ObjectTransform PreviewTr = new();
        private int pnumberOfPoints = 4;
        public int NumberOfPoints { get => pnumberOfPoints; set { pnumberOfPoints = value; OnChange(); } }
        public List<Vector2> Points = [];
        private readonly List<IntVector2> pPoints = [];

        public RegularPoligon()
        {
            OnChange();
            OnCreate();
        }

        public RegularPoligon(Vector2 position, Vector2 size, int nrOfPoints)
        {
            Transform.Position = position;
            Transform.Size = size;
            NumberOfPoints = nrOfPoints;
            OnChange();
            OnCreate();
        }


        private void OnChange()
        {
            pnumberOfPoints = (int)MathF.Max(pnumberOfPoints, 0);
            Points.Clear();
            for (int i = 0; i < pnumberOfPoints; i++)
            {
                Points.Add(new Vector2(0, 1));
                Points[i] = Geometrics.RotatePoint(Points[i], Vector2.Zero, 180 / pnumberOfPoints + i * 360 / pnumberOfPoints + Transform.GlobalAngle);
                Points[i] *= Transform.Size;
                pPoints[i] = Points[i].ToInt();
            }
            PreviewTr = Transform;
        }


        internal override void DrawOnBuffer(ObjectSpace where)
        {
            if (Enabled && View.character != Game.transparentCharacter)
            {
                if (PreviewTr.Equals(Transform))
                    OnChange();
                if (View.character != Game.transparentCharacter)
                    Draw.DrawPoligon([.. Points], Transform.GlobalPosition + Game.ScreenSize / 2, this.View);
            }
        }
    }
}
