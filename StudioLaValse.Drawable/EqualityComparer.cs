using System.Diagnostics.CodeAnalysis;

namespace StudioLaValse.Drawable
{
    /// <summary>
    /// The default key equality comparer for entities.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class EqualityComparer<TKey> : IEqualityComparer<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public EqualityComparer()
        {

        }

        /// <inheritdoc/>
        public bool Equals(TKey? x, TKey? y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] TKey obj)
        {
            return obj.GetHashCode();
        }
    }
}
