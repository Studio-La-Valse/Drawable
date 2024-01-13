namespace StudioLaValse.Drawable.Interaction.Private
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Creates an intersection between two enumerables. Elements only present in the first set are enumerated by the left enumerable, elements only present in the second enumerable are enumerated by the right enumerable, elements present in both enumerables are enumerated by the middle enumerable.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static (IEnumerable<TEntity> left, IEnumerable<TEntity> middle, IEnumerable<TEntity> right) Venn<TEntity>(this IEnumerable<TEntity> first, IEnumerable<TEntity> second) where TEntity : class, IEquatable<TEntity>
        {
            var equalityComparer = new EntityEqualityComparer<TEntity>();
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
