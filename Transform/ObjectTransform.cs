

namespace TeoEngine.Transform
{
    public class ObjectTransform : BasicTransform
    {
        private float pAngle = 0;
        public float Angle { get => pAngle; set { pAngle = value % 360; } }
        private float pGlobalAngle = 0;
        public float GlobalAngle { get => pGlobalAngle; internal set { pGlobalAngle = value % 360; } }


        public void Rotate(float angle)
        {
            Angle += angle;
        }
    }
}
