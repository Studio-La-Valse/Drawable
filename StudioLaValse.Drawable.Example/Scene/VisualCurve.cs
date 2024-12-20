using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System.ComponentModel;

namespace StudioLaValse.Drawable.Example.Scene
{
    public class VisualCurve : BaseSelectableParent<ElementId>
    {
        private readonly CurveModel curve;
        private readonly ISelectionManager<PersistentElement> selection;
        private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;
        private readonly HashSet<VisualCurveControlPoint> controlPoints = [];
        private bool isMouseOver;

        public override bool IsSelected => selection.IsSelected(curve);
        protected override bool IsMouseOver
        {
            get => isMouseOver;
            set
            {
                if (value == isMouseOver)
                {
                    return;
                }

                isMouseOver = value;
                notifyEntityChanged.Invalidate(curve.ElementId, method: Method.Deep);
            }
        }

        public VisualCurve(CurveModel curve, ISelectionManager<PersistentElement> selection, INotifyEntityChanged<ElementId> notifyEntityChanged) : base(curve.ElementId)
        {
            this.curve = curve;
            this.selection = selection;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            controlPoints.Clear();
            foreach(var controlPoint in curve.ControlPoints)
            {
                var visualPoint = new VisualCurveControlPoint(controlPoint, this, selection, notifyEntityChanged);
                controlPoints.Add(visualPoint);
                yield return visualPoint;
            }
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            var color = IsSelected ? new ColorARGB(1, 255, 0, 0) : isMouseOver ? new ColorARGB(0.5, 255, 0, 0) : ColorARGB.White;
            var list = new List<BaseDrawableElement>()
            {
                new DrawableBezierCubic(curve.First.Point, curve.Second.Point, curve.Third.Point, curve.Fourth.Point, color, 2)
            };

            if (IsSelected)
            {
                list.Add(new DrawablePolyline(curve.ControlPoints.Select(p => p.Point), ColorARGB.White, 0.5));
            }

            return list;
        }

        public override bool CaptureMouse(XY point)
        {
            var margin = 1;
            return new CubicBezierSegment(curve.First.Point, curve.Second.Point, curve.Third.Point, curve.Fourth.Point).ClosestPoint(point).DistanceTo(point) < margin;
        }

        public override BoundingBox BoundingBox()
        {
            return new BoundingBox(GetDrawableElements().OfType<DrawableBezierCubic>().Select(e => e.BoundingBox()));
        }

        public override bool Deselect()
        {
            if(controlPoints.Any(e => e.MouseIsOver))
            {
                return false;
            }

            var anyChanged = false;
            if (selection.RemoveRange(curve.ControlPoints))
            {
                anyChanged = true;
            }

            if (selection.Remove(curve))
            {
                anyChanged = true;
            }
            return anyChanged;
        }

        public override bool Select()
        {
            return selection.Add(curve);
        }
    }
}
