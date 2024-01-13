using StudioLaValse.Drawable.Interaction.Selection;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class SelectionWithKeyResponse<TEntity> : ISelectionManager<TEntity>
    {
        private readonly ISelectionManager<TEntity> source;
        private readonly Func<bool> shiftPressed;
        private readonly Func<bool> controlPressed;

        public SelectionWithKeyResponse(ISelectionManager<TEntity> source, Func<bool> shiftPressed, Func<bool> controlPressed)
        {
            this.source = source;
            this.shiftPressed = shiftPressed;
            this.controlPressed = controlPressed;
        }

        public void Add(TEntity element)
        {
            source.Add(element);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            source.AddRange(entities);
        }

        public void Clear()
        {
            if (shiftPressed() || controlPressed())
            {
                return;
            }

            source.Clear();
        }

        public IEnumerable<TEntity> GetSelection()
        {
            return source.GetSelection();
        }

        public bool IsSelected(TEntity element)
        {
            return source.IsSelected(element);
        }

        public void Remove(TEntity element)
        {
            source.Remove(element);
        }

        public void RemoveRange(IEnumerable<TEntity> element)
        {
            source.RemoveRange(element);
        }

        public void Set(TEntity element)
        {
            if (shiftPressed())
            {
                source.Add(element);
                return;
            }

            if (controlPressed())
            {
                source.Remove(element);
                return;
            }

            source.Set(element);
        }

        public void SetRange(IEnumerable<TEntity> entities)
        {
            if (shiftPressed())
            {
                source.AddRange(entities);
                return;
            }

            if (controlPressed())
            {
                source.RemoveRange(entities);
                return;
            }

            source.SetRange(entities);
        }
    }
}
