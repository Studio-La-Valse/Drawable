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

        public bool Clear()
        {
            var changedElements = selection.GetSelection().ToArray();

            if (!selection.Clear())
            {
                return false;
            }

            Emit([], changedElements);

            return true;
        }

        public bool Add(TEntity element)
        {
            if (!selection.Add(element))
            {
                return false;
            }

            Emit([element], []);

            return true;
        }

        public bool AddRange(IEnumerable<TEntity> entities)
        {
            if (!selection.AddRange(entities))
            {
                return false;
            }

            Emit(entities, []);
            return true;
        }

        public bool Remove(TEntity element)
        {
            if (!selection.Remove(element))
            {
                return false;
            }

            Emit([], [element]);
            return true;
        }

        public bool RemoveRange(IEnumerable<TEntity> entities)
        {
            if (!selection.RemoveRange(entities))
            {
                return false;
            }

            Emit([], entities);
            return true;
        }

        public bool Set(TEntity element)
        {
            var existingSelection = selection.GetSelection().ToArray();

            if (!selection.Set(element))
            {
                return false;
            }

            Emit([element], existingSelection);
            return true;
        }

        public bool SetRange(IEnumerable<TEntity> entities)
        {
            var existingSelection = selection.GetSelection().ToArray();
            if (!selection.SetRange(entities))
            {
                return false;
            }

            var (left, _, right) = existingSelection.Venn(entities, getKey);

            Emit(right, left);
            return true;
        }
    }
}
