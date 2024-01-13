using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class PipeSelection<TEntity> : IPipe where TEntity : class, IEquatable<TEntity>
    {
        private readonly IPipe source;
        private readonly IEnumerable<BaseVisualParent<TEntity>> scene;
        private readonly ISelectionManager<TEntity> selection;


        public XY LastMousePosition { get; set; } = new XY(0, 0);


        public PipeSelection(IPipe source, IEnumerable<BaseVisualParent<TEntity>> scene, ISelectionManager<TEntity> selection)
        {
            this.source = source;
            this.scene = scene;
            this.selection = selection;
        }


        public void HandleLeftMouseButtonDown()
        {
            source.HandleLeftMouseButtonDown();
        }

        public void HandleLeftMouseButtonUp()
        {
            source.HandleLeftMouseButtonUp();

            var toSelect = scene
                .OfType<BaseSelectableParent<TEntity>>()
                .Where(p => p.Respond(LastMousePosition))
                .Reverse()
                .FirstOrDefault();
            if (toSelect is null)
            {
                selection.Clear();
                return;
            }

            selection.Set(toSelect.AssociatedElement);
        }

        public void HandleSetMousePosition(XY position)
        {
            source.HandleSetMousePosition(position);

            LastMousePosition = position;
        }

        public void KeyUp(Key key)
        {
            source.KeyUp(key);
        }

        public void KeyDown(Key key)
        {
            source.KeyDown(key);

            if (key == Key.Escape)
            {
                selection.Clear();
            }
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
