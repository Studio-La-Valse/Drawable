using System.Diagnostics.CodeAnalysis;

namespace StudioLaValse.Drawable.Private
{
    /// <summary>
    /// The default key equality comparer for entities.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    internal class TreeBranchKeyEqualityComparer<TEntity, TKey> : IEqualityComparer<VisualTree<TEntity>> where TKey : IEquatable<TKey> where TEntity : class
    {
        private readonly GetKey<TEntity, TKey> keyExtractor;

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="keyExtractor"></param>
        public TreeBranchKeyEqualityComparer(GetKey<TEntity, TKey> keyExtractor)
        {
            this.keyExtractor = keyExtractor;
        }

        /// <inheritdoc/>
        public bool Equals(VisualTree<TEntity>? x, VisualTree<TEntity>? y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return keyExtractor(x.Element).Equals(keyExtractor(y.Element));
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] VisualTree<TEntity> obj)
        {
            return keyExtractor(obj.Element).GetHashCode();
        }
    }
}
