using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace Example.Model
{
    public class PointModel : PersistentElement
    {
        private double radius = 10;
        private double x = new Random().NextDouble() * 2000;
        private double y = new Random().NextDouble() * 2000;
        private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;

        public double Radius
        {
            get => radius;
            set
            {
                if(Math.Abs(radius - value) <= BaseGeometry.threshold)
                {
                    return;
                }

                radius = value;
                notifyEntityChanged.Invalidate(ElementId, NotFoundHandler.Throw, RenderMethod.Recursive);
            }
        }
        public double X
        {
            get => x; 
            set
            {
                if(x == value)
                {
                    return;
                }
                x = value;
                notifyEntityChanged.Invalidate(ElementId, NotFoundHandler.Throw, RenderMethod.Recursive);
            }
        }
        public double Y
        {
            get => y;
            set
            {
                if(y == value)
                {
                    return;
                }
                y = value;
                notifyEntityChanged.Invalidate(ElementId, NotFoundHandler.Throw, RenderMethod.Recursive);
            }
        }
        public PointModel(IKeyGenerator<int> keyGenerator, INotifyEntityChanged<ElementId> notifyEntityChanged) : base(keyGenerator)
        {
            this.notifyEntityChanged = notifyEntityChanged;
        }
    }
}
