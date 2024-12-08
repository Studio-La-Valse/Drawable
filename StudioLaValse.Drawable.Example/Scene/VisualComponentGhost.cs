using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Scene
{
    public class VisualComponentGhost : BaseVisualParent<ElementId>
    {
        private readonly VisualComponent host;

        public VisualComponentGhost(PersistentElement componentGhost, VisualComponent host) : base(componentGhost.ElementId)
        {
            this.host = host;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            return new List<BaseContentWrapper>();
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            if (host.IsSelected)
            {
                yield return new DrawableCircle(host.X, host.Y, host.Radius, ColorARGB.Transparant, new ColorARGB(1, 255, 0, 0), 5);
                yield break;
            }

            if (host.MouseIsOver)
            {
                yield return new DrawableCircle(host.X, host.Y, host.Radius, new ColorARGB(0.5, 255, 0, 0));
                yield break;
            }

            yield break;
        }
    }
}
