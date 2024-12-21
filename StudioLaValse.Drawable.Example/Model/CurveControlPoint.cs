using StudioLaValse.Drawable;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System.Runtime.InteropServices.JavaScript;

namespace Example.Model
{
    public class CurveControlPoint : PersistentElement
    {
        private XY point;
        private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;

        public XY Point => point;
        public CurveModel ControlFor { get; }


        public CurveControlPoint(IKeyGenerator<int> keyGenerator, CurveModel controlFor, INotifyEntityChanged<ElementId> notifyEntityChanged) : base(keyGenerator)
        {
            point = new XY(Random.Shared.Next(200), Random.Shared.Next(200));
            ControlFor = controlFor;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public void Set(double x, double y)
        {
            var point = new XY(x, y);
            if(this.point.DistanceTo(point) < BaseGeometry.threshold)
            {
                return;
            }

            this.point = point;
            notifyEntityChanged.Invalidate(ControlFor.ElementId, NotFoundHandler.Throw, RenderMethod.Deep);
        }
    }
}
