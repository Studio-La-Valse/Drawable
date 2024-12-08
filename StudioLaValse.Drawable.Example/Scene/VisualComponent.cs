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
    public class VisualComponent : BaseTransformableParent<PersistentElement>
    {
        private readonly ComponentModel component;
        private readonly ISelectionManager<PersistentElement> selection;
        private readonly INotifyEntityChanged<PersistentElement> notifyEntityChanged;

        public double Radius => component.Radius;
        public double X => component.X;
        public double Y => component.Y;
        public override PersistentElement Ghost => component.Ghost;
        public override bool IsSelected => selection.IsSelected(AssociatedElement);
        protected override bool IsMouseOver
        {
            get => base.IsMouseOver;
            set
            {
                if (value == base.IsMouseOver)
                {
                    return;
                }

                base.IsMouseOver = value;
                notifyEntityChanged.Invalidate(Ghost, NotFoundHandler.Throw, Method.Shallow);
            }
        }
        public bool MouseIsOver => IsMouseOver;

        public VisualComponent(ComponentModel component, ISelectionManager<PersistentElement> selection, INotifyEntityChanged<PersistentElement> notifyEntityChanged) : base(component, selection)
        {
            this.component = component;
            this.selection = selection;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            return new List<BaseContentWrapper>()
            {
                new VisualComponentGhost(Ghost, this)
            };
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            return new List<BaseDrawableElement>()
            {
                new DrawableCircle(X, Y, Radius, ColorARGB.White)
            };
        }

        public override BoundingBox BoundingBox()
        {
            return new BoundingBox(
                X - Radius / 2,
                X + Radius / 2,
                Y - Radius / 2,
                Y + Radius / 2);
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

        public override bool HandleSetMousePosition(XY position)
        {
            var distance = new XY(X, Y).DistanceTo(position);
            var radius = distance.Map(0, 500, 20, 5).Clip(5, 20);
            component.Radius = radius;

            return base.HandleSetMousePosition(position);
        }
    }
}
