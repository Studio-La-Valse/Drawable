using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class MassTransformBehavior<TKey> : IInputObserver where TKey : IEquatable<TKey>
    {
        private readonly SceneManager<TKey> sceneManager;
        private readonly double dragDelta = 2;

        private XY deltaPosition;
        private XY lastMousePosition;
        private XY lastMouseDownPosition;
        private bool leftMouseIsDown;

        public bool Dragging =>
            leftMouseIsDown && lastMousePosition.DistanceTo(lastMouseDownPosition) > dragDelta;
        public bool DirectionRight =>
            lastMousePosition.X > lastMouseDownPosition.X;


        public MassTransformBehavior(SceneManager<TKey> sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public bool HandleKeyDown(Key key)
        {
            return true;
        }

        public bool HandleKeyUp(Key key)
        {
            return true;
        }

        public bool HandleLeftMouseButtonDown()
        {
            lastMouseDownPosition = lastMousePosition;
            leftMouseIsDown = true;

            var mouseDownOnAnySelected = false;

            sceneManager.TraverseAndHandle(e =>
            {
                if (mouseDownOnAnySelected)
                {
                    return false;
                }

                if (e is BaseTransformableParent<TKey> transformable)
                {
                    if (transformable.IsSelected)
                    {
                        if (transformable.CaptureMouse(lastMouseDownPosition))
                        {
                            mouseDownOnAnySelected = true;
                        }
                    }
                }
                return true;
            });

            if (mouseDownOnAnySelected)
            {
                // We are going to move elements, prevent further propagation.
                return false;
            }

            return true;
        }

        public bool HandleMouseMove(XY position)
        {
            deltaPosition = position - lastMousePosition;

            lastMousePosition = position;

            if (!Dragging)
            {
                return true;
            }

            sceneManager.TraverseAndHandle(e =>
            {
                if (e is BaseTransformableParent<TKey> transformable)
                {
                    using var lockTransform = new LockTransform(transformable);
                    var _continue = transformable.HandleMouseMove(lastMousePosition);

                    if (transformable.IsSelected)
                    {
                        transformable.Transform(deltaPosition.X, deltaPosition.Y);
                    }

                    return _continue;
                }
                else if (e is BaseInteractiveParent<TKey> interactive)
                {
                    return interactive.HandleMouseMove(lastMousePosition);
                }
                return true;
            });

            return false;
        }

        public bool HandleLeftMouseButtonUp()
        {
            leftMouseIsDown = false;

            return true;
        }


        public bool HandleMouseWheel(double delta)
        {
            return true;
        }

        public bool HandleRightMouseButtonDown()
        {
            return true;
        }

        public bool HandleRightMouseButtonUp()
        {
            return true;
        }

        class LockTransform : IDisposable
        {
            private readonly BaseTransformableParent<TKey> baseTransformableParent;

            public LockTransform(BaseTransformableParent<TKey> baseTransformableParent)
            {
                this.baseTransformableParent = baseTransformableParent;
                baseTransformableParent.LockTransform = true;
            }
            public void Dispose()
            {
                baseTransformableParent.LockTransform = false;
            }
        }
    }
}

