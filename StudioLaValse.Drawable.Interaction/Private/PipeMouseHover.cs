using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class PipeMouseHover<TEntity> : IPipe where TEntity : class
    {
        private readonly IPipe source;
        private readonly IEnumerable<BaseVisualParent<TEntity>> sceneManager;
        private readonly INotifyEntityChanged<TEntity> entityChanged;

        public bool MouseDown { get; private set; }

        public PipeMouseHover(IPipe source, IEnumerable<BaseVisualParent<TEntity>> scene, INotifyEntityChanged<TEntity> entityChanged)
        {
            this.source = source;
            sceneManager = scene;
            this.entityChanged = entityChanged;
        }

        public void HandleSetMousePosition(XY position)
        {
            var dragging = MouseDown;

            source.HandleSetMousePosition(position);

            foreach (var element in sceneManager.OfType<BaseInteractiveParent<TEntity>>())
            {
                var isMouseOver = element.IsMouseOver;
                element.IsMouseOver = dragging ? false : element.Respond(position);
                if (isMouseOver != element.IsMouseOver)
                {
                    entityChanged.Invalidate(element.Ghost);
                }
            }
        }

        public void HandleLeftMouseButtonDown()
        {
            MouseDown = true;
            source.HandleLeftMouseButtonDown();
        }

        public void HandleLeftMouseButtonUp()
        {
            MouseDown = false;
            source.HandleLeftMouseButtonUp();
        }

        public void HandleRightMouseButtonDown()
        {
            source.HandleRightMouseButtonDown();
        }

        public void HandleRightMouseButtonUp()
        {
            source.HandleRightMouseButtonUp();
        }

        public void HandleMouseWheel(double delta)
        {
            source.HandleMouseWheel(delta);
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
