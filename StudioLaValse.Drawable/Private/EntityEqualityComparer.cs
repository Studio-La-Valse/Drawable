using System.Diagnostics.CodeAnalysis;

namespace StudioLaValse.Drawable.Private
{
    internal class EntityEqualityComparer<TEntity> : IEqualityComparer<TEntity> where TEntity : class, IEquatable<TEntity>
    {
        public bool Equals(TEntity? x, TEntity? y)
        {
            if (x is null || y is null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode([DisallowNull] TEntity obj)
        {
            return obj.GetHashCode();
        }
    }
}
