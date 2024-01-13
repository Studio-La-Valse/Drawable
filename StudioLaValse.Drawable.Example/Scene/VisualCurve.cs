using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Scene
{
    public class VisualCurve : BaseSelectableParent<PersistentElement>
    {
        private readonly CurveModel curve;
        private readonly ISelection<PersistentElement> selection;

        public VisualCurve(CurveModel curve, ISelection<PersistentElement> selection) : base(curve, selection)
        {
            this.curve = curve;
            this.selection = selection;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            var list = new List<BaseContentWrapper>(curve.ControlPoints.Select(p => new VisualCurveControlPoint(p, this, selection)));
            return list;
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            var color = IsMouseOver ? new ColorARGB(255, new ColorRGB(255, 0, 0)) : ColorARGB.White;
            var list = new List<BaseDrawableElement>()
            {
                new DrawableBezierCurve(curve.ControlPoints.Select(p => p.Point), color, 2)
            };

            if (IsSelected)
            {
                list.Add(new DrawablePolyline(curve.ControlPoints.Select(p => p.Point), ColorARGB.White, 0.5));
            }

            return list;
        }

        public override bool OnMouseMove(XY mousePosition)
        {
            return false;
        }

        public override bool Respond(XY point)
        {
            return new DrawableBezierCurve(curve.ControlPoints.Select(p => p.Point), ColorARGB.White, 2).ClosestPoint(point).DistanceTo(point) < 10;
        }

        public override BoundingBox BoundingBox()
        {
            return new DrawableBezierCurve(curve.ControlPoints.Select(p => p.Point), ColorARGB.Black, 2).GetBoundingBox();
        }
    }
}
