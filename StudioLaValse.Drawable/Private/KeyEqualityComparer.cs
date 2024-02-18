using System;
using System.Diagnostics.CodeAnalysis;

namespace StudioLaValse.Drawable.Private
{
    internal class KeyEqualityComparer<TEntity, TKey> : IEqualityComparer<TEntity> where TKey : IEquatable<TKey> where TEntity : class
    {
        private readonly GetKey<TEntity, TKey> keyExtractor;

        public KeyEqualityComparer(GetKey<TEntity, TKey> keyExtractor)  
        {
            this.keyExtractor = keyExtractor;
        }


        public bool Equals(TEntity? x, TEntity? y)
        {
            if(x == null || y == null)
            {
                return false;
            }

            return keyExtractor(x).Equals(keyExtractor(y));
        }

        public int GetHashCode([DisallowNull] TEntity obj)
        {
            return keyExtractor(obj).GetHashCode();
        }
    }
}
