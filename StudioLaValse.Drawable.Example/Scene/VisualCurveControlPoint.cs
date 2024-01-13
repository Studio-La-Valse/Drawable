using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Scene
{
    public class VisualCurveControlPoint : BaseTransformableParent<PersistentElement>
    {
        private readonly CurveControlPoint controlPoint;
        private readonly VisualCurve host;


        public override PersistentElement Ghost => controlPoint;

        public VisualCurveControlPoint(CurveControlPoint controlPoint, VisualCurve host, ISelection<PersistentElement> selection) : base(controlPoint, selection)
        {
            this.controlPoint = controlPoint;
            this.host = host;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            return new List<BaseContentWrapper>();
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            if (!host.IsSelected && !IsSelected)
            {
                return new List<BaseDrawableElement>();
            }

            var alpha = 0;
            if (IsMouseOver)
            {
                alpha += 100;
            }

            if (IsSelected)
            {
                alpha += 100;
            }

            var ghostColor = new ColorARGB(alpha, new ColorRGB(255, 0, 0));
            return new List<BaseDrawableElement>()
            {
                new DrawableCircle(controlPoint.Point, 5, ColorARGB.White),
                new DrawableCircle(controlPoint.Point, 5, ghostColor)
            };
        }

        public override bool Respond(XY point)
        {
            var distance = point.DistanceTo(controlPoint.Point);
            return distance < 5;
        }

        public override bool OnMouseMove(XY mousePosition)
        {
            return false;
        }

        public override bool Transform(double deltaX, double deltaY)
        {
            controlPoint.Set(controlPoint.Point.X + deltaX, controlPoint.Point.Y + deltaY);
            return true;
        }

        public override PersistentElement OnTransformInvalidate()
        {
            return host.AssociatedElement;
        }
    }
}
