using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class PipeSelectionBorder<TEntity> : IPipe where TEntity : class
    {
        private readonly IPipe source;
        private readonly IEnumerable<BaseVisualParent<TEntity>> scene;
        private readonly ISelectionManager<TEntity> selectionManager;
        private readonly ObservableBoundingBox boundingBox;
        private readonly INotifyEntityChanged<TEntity> entityChanged;
        private readonly double dragDelta = 2;
        private readonly HashSet<TEntity> toSelect = new();


        public XY LastMousePosition { get; set; } = new XY(0, 0);
        public XY LastMouseClickPosition { get; set; } = new XY(0, 0);
        public bool LeftMouseIsDown { get; set; }
        public bool Dragging =>
            LeftMouseIsDown && LastMousePosition.DistanceTo(LastMouseClickPosition) > dragDelta;
        public bool DirectionRight =>
            LastMousePosition.X > LastMouseClickPosition.X;

        public PipeSelectionBorder(IPipe source, IEnumerable<BaseVisualParent<TEntity>> scene, ISelectionManager<TEntity> selectionManager, ObservableBoundingBox observable, INotifyEntityChanged<TEntity> entityChanged)
        {
            this.source = source;
            this.scene = scene;
            this.selectionManager = selectionManager;
            boundingBox = observable;
            this.entityChanged = entityChanged;
        }


        public void HandleLeftMouseButtonDown()
        {
            source.HandleLeftMouseButtonDown();

            LastMouseClickPosition = LastMousePosition;
            LeftMouseIsDown = true;
        }

        public void HandleLeftMouseButtonUp()
        {
            // Hide border regardless of results.
            boundingBox.Hide();

            // Store dragging variable because it will be false when LeftMouseIsDown is set to false.
            var wasDragging = Dragging;
            LeftMouseIsDown = false;

            // Looks like it was just a click. Let the next dispatcher in the pipeline do it's thing.
            if (!wasDragging)
            {
                source.HandleLeftMouseButtonUp();
                return;
            }

            // Escape early. Drag operation has been completed but there is nothing to select.
            if (!toSelect.Any())
            {
                // Nothing to select so clear selection.
                // Don't call source.HandleLeftMouseButtonUp because that might cause a click trigger that selects the element below.
                selectionManager.Clear();
                return;
            }

            source.HandleLeftMouseButtonUp();

            // Set the selection to whatever was inside of the drag selection border. 
            selectionManager.SetRange(toSelect);
        }

        public void HandleSetMousePosition(XY position)
        {
            // Setup for dragging check. Set visibility accordingly.
            LastMousePosition = position;
            if (!Dragging)
            {
                boundingBox.Hide();
            }

            // First let the underlying dispatcher do it's thing. 
            source.HandleSetMousePosition(position);

            // Escape early
            if (!Dragging)
            {
                return;
            }

            // Prepare. 
            toSelect.Clear();

            // Set the bounding box, dont forget that the source is in canvas pixels, need to be host pixels.
            var box = new BoundingBox(LastMouseClickPosition, LastMousePosition);
            boundingBox.Set(box);

            // Search for all items under the selection box.
            var elementsInBox = scene
                .Where(p =>
                {
                    var parentBoundingBox = p.BoundingBox();

                    return DirectionRight ?
                        box.Contains(parentBoundingBox) :
                        box.Overlaps(parentBoundingBox);
                });
            foreach (var element in elementsInBox.OfType<BaseInteractiveParent<TEntity>>())
            {
                // Store the IsMouseOverProperty
                var previousMouseOver = element.IsMouseOver;

                // Set to true
                element.IsMouseOver = true;

                // Only invalidate if it was previously not true
                if (!previousMouseOver)
                {
                    entityChanged.Invalidate(element.Ghost);
                }

                // Add to list to select when mouse button is released.
                toSelect.Add(element.AssociatedElement);
            }
        }

        public void KeyUp(Key key)
        {
            source.KeyUp(key);
        }

        public void KeyDown(Key key)
        {
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
