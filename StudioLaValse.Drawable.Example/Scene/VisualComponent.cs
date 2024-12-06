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

        public double Radius { get; set; } = 10;
        public double X { get; set; } = new Random().NextDouble() * 2000;
        public double Y { get; set; } = new Random().NextDouble() * 2000;
        public override PersistentElement Ghost => component.Ghost;
        public override bool IsSelected => selection.IsSelected(AssociatedElement);
        public bool MouseIsOver => base.IsMouseOver;

        public VisualComponent(ComponentModel component, ISelectionManager<PersistentElement> selection) : base(component, selection)
        {
            this.component = component;
            this.selection = selection;
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

        public override bool Transform(double deltaX, double deltaY)
        {
            X += deltaX;
            Y += deltaY;
            return true;
        }

        public override bool HandleSetMousePosition(XY position, Queue<InvalidationRequest<PersistentElement>> invalidationRequests)
        {
            var oldRadius = Radius;
            var distance = new XY(X, Y).DistanceTo(position);
            var radius = distance.Map(0, 500, 20, 5).Clip(5, 20);
            Radius = radius;

            if(oldRadius != Radius)
            {
                invalidationRequests.Enqueue(new InvalidationRequest<PersistentElement>(AssociatedElement, Method: Method.Recursive));
            }

            return base.HandleSetMousePosition(position, invalidationRequests);
        }
    }
}
