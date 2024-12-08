using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Scene
{
    public class VisualCurveControlPoint : BaseTransformableParent<ElementId>
    {
        private readonly CurveControlPoint controlPoint;
        private readonly VisualCurve host;
        private readonly ISelectionManager<PersistentElement> selection;

        public VisualCurveControlPoint(CurveControlPoint controlPoint, VisualCurve host, ISelectionManager<PersistentElement> selection) : base(controlPoint.ElementId)
        {
            this.controlPoint = controlPoint;
            this.host = host;
            this.selection = selection;
        }

        public override bool IsSelected => selection.IsSelected(controlPoint);

        public override bool Deselect()
        {
            return selection.Remove(controlPoint);
        }

        public override bool Select()
        {
            return selection.Add(controlPoint);
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

            var ghostColor = new ColorARGB(alpha, 255, 0, 0);
            return new List<BaseDrawableElement>()
            {
                new DrawableCircle(controlPoint.Point, 5, ColorARGB.White),
                new DrawableCircle(controlPoint.Point, 5, ghostColor)
            };
        }

        public override void Transform(double deltaX, double deltaY)
        {
            controlPoint.Set(controlPoint.Point.X + deltaX, controlPoint.Point.Y + deltaY);
        }
    }
}
