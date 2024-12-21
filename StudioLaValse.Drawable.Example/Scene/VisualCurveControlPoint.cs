using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using Example.Model;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System.ComponentModel;
using System.Security.Cryptography;

namespace Example.Scene
{
    public class VisualCurveControlPoint : BaseTransformableParent<ElementId>
    {
        private readonly CurveControlPoint controlPoint;
        private readonly VisualCurve host;
        private readonly ISelectionManager<PersistentElement> selection;
        private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;
        private bool isMouseOver;

        public VisualCurveControlPoint(CurveControlPoint controlPoint, VisualCurve host, ISelectionManager<PersistentElement> selection, INotifyEntityChanged<ElementId> notifyEntityChanged) : base(controlPoint.ElementId)
        {
            this.controlPoint = controlPoint;
            this.host = host;
            this.selection = selection;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public override bool IsSelected => selection.IsSelected(controlPoint);

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
                notifyEntityChanged.Invalidate(host.Key, renderMethod: RenderMethod.Deep);
            }
        }
        public bool MouseIsOver => IsMouseOver;

        public override bool Deselect()
        {
            return selection.Remove(controlPoint);
        }

        public override bool Select()
        {
            var hasChanged = selection.Add(controlPoint);

            if (selection.Add(controlPoint.ControlFor))
            {
                hasChanged = true;
            }

            if (hasChanged)
            {
                notifyEntityChanged.Invalidate(controlPoint.ControlFor.ElementId, renderMethod: RenderMethod.Deep);
            }

            return hasChanged;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            yield break;
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            if (!host.IsSelected)
            {
                yield break;
            }

            if (IsSelected)
            {
                yield return new DrawableCircle(controlPoint.Point.X, controlPoint.Point.Y, 2, ColorARGB.White, new ColorARGB(1, 255, 0, 0), 0.5);
                yield break;
            }

            if (IsMouseOver)
            {
                yield return new DrawableCircle(controlPoint.Point.X, controlPoint.Point.Y, 2, new ColorARGB(0.5, 255, 0, 0));
                yield break;
            }

            yield return new DrawableCircle(controlPoint.Point.X, controlPoint.Point.Y, 2, ColorARGB.White);
        }

        public override void Transform(double deltaX, double deltaY)
        {
            controlPoint.Set(controlPoint.Point.X + deltaX, controlPoint.Point.Y + deltaY);
        }
    }
}
