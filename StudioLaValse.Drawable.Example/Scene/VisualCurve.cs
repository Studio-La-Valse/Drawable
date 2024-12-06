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
        private readonly ISelectionManager<PersistentElement> selection;

        public VisualCurve(CurveModel curve, ISelectionManager<PersistentElement> selection) : base(curve, selection)
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
            var color = IsMouseOver ? new ColorARGB(255, 255, 0, 0) : ColorARGB.White;
            var list = new List<BaseDrawableElement>()
            {
                new DrawableBezierQuadratic(curve.First.Point, curve.Second.Point, curve.Third.Point, curve.Fourth.Point, color, 2)
            };

            if (IsSelected)
            {
                list.Add(new DrawablePolyline(curve.ControlPoints.Select(p => p.Point), ColorARGB.White, 0.5));
            }

            return list;
        }

        public override bool CaptureMouse(XY point)
        {
            var margin = 2;
            return new CubicBezierSegment(curve.First.Point, curve.Second.Point, curve.Third.Point, curve.Fourth.Point).ClosestPoint(point).DistanceTo(point) < margin;
        }

        public override BoundingBox BoundingBox()
        {
            return new BoundingBox(GetDrawableElements().OfType<DrawableBezierQuadratic>().Select(e => e.GetBoundingBox()));
        }
    }
}
