using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class PipeRerender<TEntity> : IPipe where TEntity : class, IEquatable<TEntity>
    {
        private readonly IPipe source;
        private readonly INotifyEntityChanged<TEntity> observable;

        public PipeRerender(IPipe source, INotifyEntityChanged<TEntity> observable)
        {
            this.source = source;
            this.observable = observable;
        }

        public void React()
        {
            observable.RenderChanges();
        }

        public void HandleLeftMouseButtonDown()
        {
            source.HandleLeftMouseButtonDown();
            React();
        }

        public void HandleLeftMouseButtonUp()
        {
            source.HandleLeftMouseButtonUp();
            React();
        }

        public void HandleSetMousePosition(XY position)
        {
            source.HandleSetMousePosition(position);
            React();
        }

        public void KeyUp(Key key)
        {
            source.KeyUp(key);
            React();
        }

        public void KeyDown(Key key)
        {
            source.KeyDown(key);
            React();
        }

        public void HandleRightMouseButtonDown()
        {
            source.HandleRightMouseButtonDown();
            React();
        }

        public void HandleRightMouseButtonUp()
        {
            source.HandleRightMouseButtonUp();
            React();
        }

        public void HandleMouseWheel(double delta)
        {
            source.HandleMouseWheel(delta);
            React();
        }
    }
}
