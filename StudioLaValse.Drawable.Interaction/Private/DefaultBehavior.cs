using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class DefaultBehavior<TKey> : IInputObserver where TKey : IEquatable<TKey>
    {
        private readonly SceneManager<TKey> sceneManager;

        public DefaultBehavior(SceneManager<TKey> sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public bool HandleKeyDown(Key key)
        {
            var _continue = TraverseAndHandleBehavior(e => e.HandleKeyDown(key));
            return _continue;
        }

        public bool HandleKeyUp(Key key)
        {
            var _continue = TraverseAndHandleBehavior(e => e.HandleKeyUp(key));
            return _continue;
        }

        public bool HandleLeftMouseButtonDown()
        {
            var _continue = TraverseAndHandleBehavior(e => e.HandleLeftMouseButtonDown());
            return _continue;
        }

        public bool HandleLeftMouseButtonUp()
        {
            var _continue = TraverseAndHandleBehavior(e => e.HandleLeftMouseButtonUp());
            return _continue;
        }

        public bool HandleMouseMove(XY position)
        {
            var _continue = TraverseAndHandleBehavior(e => e.HandleMouseMove(position));
            return _continue;
        }

        public bool HandleMouseWheel(double delta)
        {
            var _continue = TraverseAndHandleBehavior(e => e.HandleMouseWheel(delta));
            return _continue;
        }

        public bool HandleRightMouseButtonDown()
        {
            var _continue = TraverseAndHandleBehavior(e => e.HandleRightMouseButtonDown());
            return _continue;
        }

        public bool HandleRightMouseButtonUp()
        {
            var _continue = TraverseAndHandleBehavior(e => e.HandleRightMouseButtonUp());
            return _continue;
        }

        /// <summary>
        /// Travels down the visual tree and does some behavior.
        /// </summary>
        /// <param name="handleBehavior"></param>
        protected bool TraverseAndHandleBehavior(Func<IInputObserver, bool> handleBehavior)
        {
            return sceneManager.TraverseAndHandle(e =>
            {
                if (e is IInputObserver behavior)
                {
                    var result = handleBehavior(behavior);

                    if (!result)
                    {
                        return false;
                    }
                }

                return true;
            });
        }
    }
}

