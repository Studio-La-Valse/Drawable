using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Scene
{
    public class VisualComponent : BaseTransformableParent<PersistentElement>
    {
        private readonly ComponentModel component;

        public double Radius { get; set; } = 10;
        public double X { get; set; } = new Random().NextDouble() * 2000;
        public double Y { get; set; } = new Random().NextDouble() * 2000;
        public override PersistentElement Ghost => component.Ghost;


        public VisualComponent(ComponentModel component, ISelection<PersistentElement> selection) : base(component, selection)
        {
            this.component = component;
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
                new DrawableCircle(X, Y, Radius, new ColorARGB(255, ColorRGB.White))
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

        public override bool OnMouseMove(XY mousePosition)
        {
            var oldRadius = Radius;
            var distance = new XY(X, Y).DistanceTo(mousePosition);
            var radius = distance.Map(0, 500, 20, 5).Clip(5, 20);
            Radius = radius;

            return oldRadius != radius;
        }

        public override bool Transform(double deltaX, double deltaY)
        {
            X += deltaX;
            Y += deltaY;
            return true;
        }

        public override bool Respond(XY point)
        {
            return point.DistanceTo(new XY(X, Y)) < Radius;
        }
    }
}
