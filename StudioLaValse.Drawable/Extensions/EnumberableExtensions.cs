namespace StudioLaValse.Drawable
{
    /// <summary>
    /// Extensions for enumerables.
    /// </summary>
    public static class EnumberableExtensions
    {
        /// <summary>
        /// Select items recursively. Breadth-first travel.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> SelectBreadth<T>(this T subject, Func<T, IEnumerable<T>> selector)
        {
            var stillToProcess = new Queue<T>();
            stillToProcess.Enqueue(subject);

            while (stillToProcess.Count > 0)
            {
                var item = stillToProcess.Dequeue();
                yield return item;
                foreach (var child in selector(item))
                {
                    stillToProcess.Enqueue(child);
                }
            }
        }

        /// <summary>
        /// Select items recursively using depth-first traversal.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> SelectDepth<T>(this T subject, Func<T, IEnumerable<T>> selector)
        {
            var stillToProcess = new Stack<T>();
            stillToProcess.Push(subject);

            while (stillToProcess.Count > 0)
            {
                var item = stillToProcess.Pop();
                yield return item;
                foreach (var child in selector(item))
                {
                    stillToProcess.Push(child);
                }
            }
        }

        /// <summary>
        /// Do something for each item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }
    }
}
