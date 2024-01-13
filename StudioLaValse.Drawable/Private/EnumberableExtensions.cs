namespace StudioLaValse.Drawable.Private
{
    internal static class EnumberableExtensions
    {
        public static IEnumerable<T> SelectRecursive<T>(this T subject, Func<T, IEnumerable<T>> selector)
        {
            var stillToProcess = new Queue<T>();
            stillToProcess.Enqueue(subject);

            while (stillToProcess.Count > 0)
            {
                T item = stillToProcess.Dequeue();
                yield return item;
                foreach (T child in selector(item))
                {
                    stillToProcess.Enqueue(child);
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
    }
}
