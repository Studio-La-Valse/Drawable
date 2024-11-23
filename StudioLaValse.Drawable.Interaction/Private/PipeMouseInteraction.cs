using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class PipeMouseInteraction<TEntity> : IBehavior where TEntity : class
    {
        private readonly IBehavior source;
        private readonly IEnumerable<BaseVisualParent<TEntity>> scene;
        private readonly INotifyEntityChanged<TEntity> entityChanged;

        public PipeMouseInteraction(IBehavior source, IEnumerable<BaseVisualParent<TEntity>> scene, INotifyEntityChanged<TEntity> entityChanged)
        {
            this.source = source;
            this.scene = scene;
            this.entityChanged = entityChanged;
        }

        public void HandleLeftMouseButtonDown()
        {
            source.HandleLeftMouseButtonDown();
        }

        public void HandleLeftMouseButtonUp()
        {
            source.HandleLeftMouseButtonUp();
        }

        public void HandleMouseWheel(double delta)
        {
            source.HandleMouseWheel(delta);
        }

        public void HandleRightMouseButtonDown()
        {
            source.HandleRightMouseButtonDown();
        }

        public void HandleRightMouseButtonUp()
        {
            source.HandleRightMouseButtonUp();
        }

        public void HandleSetMousePosition(XY position)
        {
            source.HandleSetMousePosition(position);

            var elements = scene.OfType<BaseInteractiveParent<TEntity>>().Where(e => e.OnMouseMove(position));
            foreach (var element in elements)
            {
                entityChanged.Invalidate(element.AssociatedElement, method: Method.Recursive);
            }
        }

        public void KeyDown(Key key)
        {
            source.KeyDown(key);
        }

        public void KeyUp(Key key)
        {
            source.KeyUp(key);
        }
    }
}
