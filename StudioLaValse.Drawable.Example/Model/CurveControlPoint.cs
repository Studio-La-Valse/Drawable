using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Model
{
    public class CurveControlPoint : PersistentElement
    {
        private XY point;
        public XY Point => point;
        public CurveModel ControlFor { get; }

        public CurveControlPoint(IKeyGenerator<int> keyGenerator, XY point, CurveModel controlFor) : base(keyGenerator)
        {
            this.point = point;
            ControlFor = controlFor;
        }

        public void Set(double x, double y)
        {
            point = new XY(x, y);
        }
    }
}
