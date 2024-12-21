using System.Diagnostics.CodeAnalysis;

namespace StudioLaValse.Drawable.Private
{
    /// <summary>
    /// The default key equality comparer for entities.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    internal class TreeBranchKeyEqualityComparer<TKey> : IEqualityComparer<VisualTree<TKey>> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public TreeBranchKeyEqualityComparer()
        {

        }

        /// <inheritdoc/>
        public bool Equals(VisualTree<TKey>? x, VisualTree<TKey>? y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Key.Equals(y.Key);
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] VisualTree<TKey> obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
