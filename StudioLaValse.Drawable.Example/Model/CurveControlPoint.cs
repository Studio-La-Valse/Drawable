using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Model
{
    public class CurveControlPoint : PersistentElement
    {
        private XY point;
        public XY Point => point;
        public CurveModel ControlFor { get; }

        public CurveControlPoint(IKeyGenerator<int> keyGenerator, CurveModel controlFor) : base(keyGenerator)
        {
            point = new XY(Random.Shared.Next(200), Random.Shared.Next(200));
            ControlFor = controlFor;
        }

        public void Set(double x, double y)
        {
            point = new XY(x, y);
        }
    }
}
