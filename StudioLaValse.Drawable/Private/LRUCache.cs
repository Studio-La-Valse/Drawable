namespace StudioLaValse.Drawable.Text
{
    internal class LRUCache<TKey, TValue> where TKey : IEquatable<TKey>
    {
        private readonly int capacity;
        private readonly Dictionary<TKey, LinkedListNode<CacheItem>> cache;
        private readonly LinkedList<CacheItem> lruList;

        public LRUCache(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentException("Capacity must be greater than zero.");
            }

            this.capacity = capacity;
            this.cache = new Dictionary<TKey, LinkedListNode<CacheItem>>(capacity);
            this.lruList = new LinkedList<CacheItem>();
        }

        public bool TryGet(TKey key, out TValue value)
        {
            value = default!;

            if (cache.TryGetValue(key, out var node))
            {
                // Move the accessed node to the front of the LRU list
                lruList.Remove(node);
                lruList.AddFirst(node);
                value = node.Value.Value;
                return true;
            }

            return false;
        }

        public void Set(TKey key, TValue value)
        {
            if (cache.TryGetValue(key, out var node))
            {
                // Remove the existing node
                lruList.Remove(node);
                cache.Remove(key);
            }
            else if (cache.Count >= capacity)
            {
                // Remove the least recently used item from the cache
                var lru = lruList.Last!;
                lruList.RemoveLast();
                cache.Remove(lru.Value.Key);
            }

            // Add the new node to the front of the LRU list
            var cacheItem = new CacheItem(key, value);
            var cacheNode = new LinkedListNode<CacheItem>(cacheItem);
            lruList.AddFirst(cacheNode);
            cache[key] = cacheNode;
        }

        private record CacheItem(TKey Key, TValue Value);
    }
}
