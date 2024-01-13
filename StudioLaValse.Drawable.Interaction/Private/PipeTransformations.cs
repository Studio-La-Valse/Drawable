using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class PipeTransformations<TEntity> : IPipe where TEntity : class, IEquatable<TEntity>
    {
        private readonly IPipe source;
        private readonly ISelectionManager<TEntity> selection;
        private readonly IEnumerable<BaseVisualParent<TEntity>> scene;
        private readonly INotifyEntityChanged<TEntity> entityChanged;
        private readonly double dragDelta = 2;
        private readonly HashSet<TEntity> transformingElements = new HashSet<TEntity>();
        private readonly HashSet<TEntity> transformedElements = new HashSet<TEntity>();

        public XY LastMousePosition { get; set; } = new XY(0, 0);
        public XY LastMouseClickPosition { get; set; } = new XY(0, 0);
        public bool LeftMouseIsDown { get; set; }
        public bool Dragging =>
            LeftMouseIsDown && LastMousePosition.DistanceTo(LastMouseClickPosition) > dragDelta;
        public double TotalDeltaX { get; set; }
        public double TotalDeltaY { get; set; }


        public PipeTransformations(IPipe source, ISelectionManager<TEntity> selection, IEnumerable<BaseVisualParent<TEntity>> sceneManager, INotifyEntityChanged<TEntity> entityChanged)
        {
            this.source = source;
            this.selection = selection;
            scene = sceneManager;
            this.entityChanged = entityChanged;
        }


        public void HandleLeftMouseButtonDown()
        {
            // Prepare
            transformingElements.Clear();
            transformedElements.Clear();

            LastMouseClickPosition = LastMousePosition;
            LeftMouseIsDown = true;

            TotalDeltaX = 0;
            TotalDeltaY = 0;

            // Look for transformable items beneath the cursor. If nothing is there, abort let the next dispatcher in the pipeline do it's thing.
            var elementBelowMouse = scene
                .OfType<BaseTransformableParent<TEntity>>()
                .Where(p => p.Respond(LastMousePosition))
                .FirstOrDefault()?.AssociatedElement;
            if (elementBelowMouse is null)
            {
                source.HandleLeftMouseButtonDown();
                return;
            }

            // If the item beneath the cursor was not selected, clear the selection and set it to that item.
            if (!selection.IsSelected(elementBelowMouse))
            {
                selection.Set(elementBelowMouse);
            }

            // We will transform the selection. 
            foreach (var element in selection.GetSelection())
            {
                transformingElements.Add(element);
            }
        }

        public void HandleLeftMouseButtonUp()
        {
            var elementsHaveBeenTransformed = transformedElements.Any();

            // Cleanup
            LeftMouseIsDown = false;
            transformingElements.Clear();
            transformedElements.Clear();

            TotalDeltaX = 0;
            TotalDeltaY = 0;

            // Only let the next dispatcher in the pipeline do it's thing if no elements have been transformed.
            if (!elementsHaveBeenTransformed)
            {
                source.HandleLeftMouseButtonUp();
                return;
            }
        }

        public void HandleSetMousePosition(XY position)
        {
            var dragDelta = position - LastMousePosition;
            LastMousePosition = position;

            // No drag happening, abort early.
            if (!Dragging)
            {
                source.HandleSetMousePosition(position);
                return;
            }

            // Nothing to transform, abort early.
            if (!transformingElements.Any())
            {
                source.HandleSetMousePosition(position);
                return;
            }

            TotalDeltaX += dragDelta.X;
            TotalDeltaY += dragDelta.Y;

            // Apply transformation.
            foreach (var element in transformingElements)
            {
                var elementInScene = scene.LocateOrThrow(element);
                if (elementInScene is not BaseTransformableParent<TEntity> transformableElement)
                {
                    continue;
                }

                if (transformableElement.Transform(dragDelta.X, dragDelta.Y))
                {
                    var elementToInvalidate = transformableElement.OnTransformInvalidate();
                    entityChanged.Invalidate(elementToInvalidate);
                    transformedElements.Add(element);
                }
            }
        }

        public void KeyUp(Key key)
        {
            source.KeyUp(key);
        }

        public void KeyDown(Key key)
        {
            if (key == Key.Escape)
            {
                // Reverse transformation.
                foreach (var element in transformingElements)
                {
                    var elementInScene = scene.LocateOrThrow(element);
                    if (elementInScene is not BaseTransformableParent<TEntity> transformableElement)
                    {
                        continue;
                    }

                    if (transformableElement.Transform(-TotalDeltaX, -TotalDeltaY))
                    {
                        var elementToInvalidate = transformableElement.OnTransformInvalidate();
                        entityChanged.Invalidate(elementToInvalidate);
                        transformedElements.Add(element);
                    }
                }

                // Prevent the happening again...
                TotalDeltaX = 0;
                TotalDeltaY = 0;
            }

            source.KeyDown(key);
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
    }
}
