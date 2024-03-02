namespace StudioLaValse.Drawable.Interaction.Private
{
    internal static class EnumerableExtensions
    {
        public static (IEnumerable<TEntity> left, IEnumerable<TEntity> middle, IEnumerable<TEntity> right) Venn<TEntity, TKey>(this IEnumerable<TEntity> first, IEnumerable<TEntity> second, GetKey<TEntity, TKey> getKey) where TEntity : class where TKey : IEquatable<TKey>
        {
            var equalityComparer = new KeyEqualityComparer<TEntity, TKey>(getKey);
            var left = new HashSet<TEntity>(equalityComparer);
            var middle = new HashSet<TEntity>(equalityComparer);
            var right = new HashSet<TEntity>(equalityComparer);

            foreach (var element in first)
            {
                if (second.Contains(element))
                {
                    middle.Add(element);
                    continue;
                }

                left.Add(element);
            }

            foreach (var element in second)
            {
                if (first.Contains(element))
                {
                    middle.Add(element);
                    continue;
                }

                right.Add(element);
            }

            return (left, middle, right);
        }
    }
}
