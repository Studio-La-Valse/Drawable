using StudioLaValse.Drawable.Interaction.Selection;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class SelectionWithChangedHandler<TEntity, TKey> : ISelectionManager<TEntity> where TEntity : class where TKey: IEquatable<TKey>
    {
        private readonly ISelectionManager<TEntity> selection;
        private readonly Action<IEnumerable<TEntity>, IEnumerable<TEntity>> action;
        private readonly GetKey<TEntity, TKey> getKey;

        public SelectionWithChangedHandler(ISelectionManager<TEntity> selection, Action<IEnumerable<TEntity>, IEnumerable<TEntity>> action, GetKey<TEntity, TKey> getKey)
        {
            this.selection = selection;
            this.action = action;
            this.getKey = getKey;
        }

        public IEnumerable<TEntity> GetSelection() => selection.GetSelection();

        public bool IsSelected(TEntity element) => selection.IsSelected(element);

        public void Emit(IEnumerable<TEntity> left, IEnumerable<TEntity> right)
        {
            action(left, right);
        }

        public void Clear()
        {
            var changedElements = selection.GetSelection().ToArray();

            selection.Clear();

            if (!changedElements.Any())
            {
                return;
            }

            Emit(Array.Empty<TEntity>(), changedElements);
        }

        public void Add(TEntity element)
        {
            selection.Add(element);
            Emit(new[] { element }, Array.Empty<TEntity>());
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            selection.AddRange(entities);
            Emit(entities, Array.Empty<TEntity>());
        }

        public void Remove(TEntity element)
        {
            selection.Remove(element);
            Emit(Array.Empty<TEntity>(), new[] { element });
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            selection.RemoveRange(entities);
            Emit(Array.Empty<TEntity>(), entities);
        }

        public void Set(TEntity element)
        {
            var existingSelection = selection.GetSelection().ToArray();
            selection.Set(element);
            Emit(new[] { element }, existingSelection);
        }

        public void SetRange(IEnumerable<TEntity> entities)
        {
            var existingSelection = selection.GetSelection().ToArray();
            var (left, _, right) = existingSelection.Venn(entities, getKey);
            selection.SetRange(entities);
            Emit(right, left);
        }
    }
}
