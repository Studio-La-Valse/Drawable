using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Scene
{
    public class VisualComponentGhost : BaseVisualParent<PersistentElement>
    {
        private readonly VisualComponent host;

        public VisualComponentGhost(PersistentElement componentGhost, VisualComponent host) : base(componentGhost)
        {
            this.host = host;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            return new List<BaseContentWrapper>();
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            var elements = new List<BaseDrawableElement>();
            if (host.IsSelected)
            {
                elements.Add(new DrawableCircle(host.X, host.Y, host.Radius, ColorARGB.Transparant, new ColorARGB(255, 255, 0, 0), 5));
            }

            if (host.IsMouseOver)
            {
                elements.Add(new DrawableCircle(host.X, host.Y, host.Radius, new ColorARGB(100, 255, 0, 0)));
            }

            return elements;
        }
    }
}
