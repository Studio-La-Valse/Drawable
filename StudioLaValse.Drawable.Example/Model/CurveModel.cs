using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Model
{
    public class CurveModel : PersistentElement
    {
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly ICollection<CurveControlPoint> curvePoints = new List<CurveControlPoint>();

        public IEnumerable<CurveControlPoint> ControlPoints => curvePoints;

        public CurveModel(IKeyGenerator<int> keyGenerator) : base(keyGenerator)
        {
            this.keyGenerator = keyGenerator;
        }


        public void AddControlPoint(XY point)
        {
            var controlPoint = new CurveControlPoint(keyGenerator, point, this);
            curvePoints.Add(controlPoint);
        }

        public void RemoveControlPoint(CurveControlPoint controlPoint)
        {
            curvePoints.Remove(controlPoint);
        }
    }
}
