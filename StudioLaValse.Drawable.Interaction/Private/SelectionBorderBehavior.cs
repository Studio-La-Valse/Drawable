using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class SelectionBorderBehavior<TKey> : IInputObserver where TKey : IEquatable<TKey>
    {
        private readonly SceneManager<TKey> sceneManager;
        private readonly SelectionBorder selectionBorder;
        private readonly double dragDelta = 2;

        private BoundingBox? lastBoundingBox;
        private XY deltaPosition;
        private XY lastMousePosition;
        private XY lastMouseDownPosition;
        private bool leftMouseIsDown;

        public bool Dragging =>
            leftMouseIsDown && lastMousePosition.DistanceTo(lastMouseDownPosition) > dragDelta;
        public bool DirectionRight =>
            lastMousePosition.X > lastMouseDownPosition.X;


        public SelectionBorderBehavior(SceneManager<TKey> sceneManager, SelectionBorder selectionBorder)
        {
            this.sceneManager = sceneManager;
            this.selectionBorder = selectionBorder;
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

            lastBoundingBox = null;

            var elementOnPoint = false;
            sceneManager.TraverseAndHandle(e =>
            {
                if (elementOnPoint)
                {
                    return false;
                }

                if (e is BaseSelectableParent<TKey> selectable && selectable.CaptureMouse(lastMouseDownPosition))
                {
                    elementOnPoint = true;
                    return false;
                }

                return true;
            });

            if (elementOnPoint)
            {
                return true;
            }

            lastBoundingBox = new BoundingBox(lastMouseDownPosition, lastMousePosition);

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

            if (!lastBoundingBox.HasValue)
            {
                return true;
            }

            lastBoundingBox = new BoundingBox(lastMouseDownPosition, lastMousePosition);
            selectionBorder.Set(lastBoundingBox.Value);

            sceneManager.TraverseAndHandle(e =>
            {
                if (e is BaseSelectableParent<TKey> selectable)
                {
                    var overlap = DirectionRight ?
                        lastBoundingBox.Value.Contains(selectable.BoundingBox()) :
                        selectable.BoundingBox().Overlaps(lastBoundingBox.Value);
                    if (overlap)
                    {
                        selectable.OnMouseEnter();
                    }
                    else
                    {
                        selectable.OnMouseLeave();
                    }
                }

                return true;
            });

            return false;
        }

        public bool HandleLeftMouseButtonUp()
        {
            if (Dragging && lastBoundingBox.HasValue)
            {
                sceneManager.TraverseAndHandle(e =>
                {
                    if (e is BaseSelectableParent<TKey> selectable)
                    {
                        var select = DirectionRight ?
                            lastBoundingBox.Value.Contains(selectable.BoundingBox()) :
                            selectable.BoundingBox().Overlaps(lastBoundingBox.Value);
                        if (select)
                        {
                            selectable.Select();
                        }
                        else
                        {
                            selectable.Deselect();
                        }
                    }
                    return true;
                });
            }

            lastBoundingBox = null;
            leftMouseIsDown = false;
            selectionBorder.Hide();

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
    }
}

