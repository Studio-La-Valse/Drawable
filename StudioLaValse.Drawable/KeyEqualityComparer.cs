using System;
using System.Diagnostics.CodeAnalysis;

namespace StudioLaValse.Drawable
{
    /// <summary>
    /// The default key equality comparer for entities.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class KeyEqualityComparer<TEntity, TKey> : IEqualityComparer<TEntity> where TKey : IEquatable<TKey> where TEntity : class
    {
        private readonly GetKey<TEntity, TKey> keyExtractor;

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="keyExtractor"></param>
        public KeyEqualityComparer(GetKey<TEntity, TKey> keyExtractor)
        {
            this.keyExtractor = keyExtractor;
        }

        /// <inheritdoc/>
        public bool Equals(TEntity? x, TEntity? y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return keyExtractor(x).Equals(keyExtractor(y));
        }

        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] TEntity obj)
        {
            return keyExtractor(obj).GetHashCode();
        }
    }
}
