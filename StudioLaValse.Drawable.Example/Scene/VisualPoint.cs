using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Interaction;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System.Drawing;

namespace StudioLaValse.Drawable.Example.Scene
{
    public class VisualPoint : BaseTransformableParent<ElementId>
    {
        private readonly PointModel component;
        private readonly ISelectionManager<PersistentElement> selection;
        private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;
        private bool isMouseOver;

        public double Radius => component.Radius;
        public double X => component.X;
        public double Y => component.Y;
        public override bool IsSelected => selection.IsSelected(component);

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
                notifyEntityChanged.Invalidate(component.ElementId, method: Method.Shallow);
            }
        }

        public VisualPoint(PointModel component, ISelectionManager<PersistentElement> selection, INotifyEntityChanged<ElementId> notifyEntityChanged) : base(component.ElementId)
        {
            this.component = component;
            this.selection = selection;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            yield break;
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            if (IsSelected)
            {
                yield return new DrawableCircle(X, Y, Radius, ColorARGB.White, new ColorARGB(1, 255, 0, 0), 5);
                yield break;
            }

            if (isMouseOver)
            {
                yield return new DrawableCircle(X, Y, Radius, new ColorARGB(1, 255, 127, 127));
                yield break;
            }

            yield return new DrawableCircle(X, Y, Radius, ColorARGB.White);
            yield break;
        }

        public override BoundingBox BoundingBox()
        {
            return new BoundingBox(
                X - Radius / 2,
                X + Radius / 2,
                Y - Radius / 2,
                Y + Radius / 2);
        }

        public override bool Deselect()
        {
            return selection.Remove(component);
        }

        public override bool Select()
        {
            return selection.Add(component);
        }

        public override void Transform(double deltaX, double deltaY)
        {
            component.X += deltaX;
            component.Y += deltaY;
        }

        public override bool CaptureMouse(XY point)
        {
            var distance = point.DistanceTo(new XY(X, Y));
            return distance <= Radius;
        }

        public override bool HandleMouseMove(XY position)
        {
            var distance = new XY(X, Y).DistanceTo(position);
            var radius = distance.Map(0, 500, 20, 5).Clip(5, 20);
            component.Radius = radius;

            return base.HandleMouseMove(position);
        }
    }
}
