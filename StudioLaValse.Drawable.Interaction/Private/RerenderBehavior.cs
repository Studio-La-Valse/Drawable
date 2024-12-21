using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class RerenderBehavior<TKey> : IInputObserver where TKey : IEquatable<TKey>
    {
        private readonly INotifyEntityChanged<TKey> notifyEntityChanged;

        public RerenderBehavior(INotifyEntityChanged<TKey> notifyEntityChanged)
        {
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public bool HandleKeyDown(Key key)
        {
            notifyEntityChanged.RenderChanges();
            return true;
        }

        public bool HandleKeyUp(Key key)
        {
            notifyEntityChanged.RenderChanges();
            return true;
        }

        public bool HandleLeftMouseButtonDown()
        {
            notifyEntityChanged.RenderChanges();
            return true;
        }

        public bool HandleLeftMouseButtonUp()
        {
            notifyEntityChanged.RenderChanges();
            return true;
        }

        public bool HandleMouseMove(XY position)
        {
            notifyEntityChanged.RenderChanges();
            return true;
        }

        public bool HandleMouseWheel(double delta)
        {
            notifyEntityChanged.RenderChanges();
            return true;
        }

        public bool HandleRightMouseButtonDown()
        {
            notifyEntityChanged.RenderChanges();
            return true;
        }

        public bool HandleRightMouseButtonUp()
        {
            notifyEntityChanged.RenderChanges();
            return true;
        }
    }
}

