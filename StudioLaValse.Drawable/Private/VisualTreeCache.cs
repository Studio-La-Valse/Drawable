using StudioLaValse.Drawable.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace StudioLaValse.Drawable.Private
{
    internal class VisualTreeCache<TKey> where TKey : IEquatable<TKey>
    {

        private readonly Dictionary<VisualTree<TKey>, InvalidationRequest<TKey>> dict;

        public VisualTreeCache()
        {
            var equalityComparer = new TreeBranchKeyEqualityComparer<TKey>();
            dict = new Dictionary<VisualTree<TKey>, InvalidationRequest<TKey>>(equalityComparer);
        }

        public void Rebuild(VisualTree<TKey> visualTree, Dictionary<TKey, InvalidationRequest<TKey>> invalidationRequests, out IEnumerable<InvalidationRequest<TKey>> notFound)
        {
            dict.Clear();

            void traverse(VisualTree<TKey> _visualTree, bool parentFound)
            {
                var element = _visualTree.Key;

                var contains = invalidationRequests.TryGetValue(element, out var invalidationRequest);
                if (contains)
                {
                    if (!parentFound)
                    {
                        Add(_visualTree, invalidationRequest!);
                    }
                    invalidationRequests.Remove(element);
                }

                if (invalidationRequests.Count == 0)
                {
                    return;
                }

                foreach (var branch in _visualTree.ChildBranches)
                {
                    traverse(branch, contains);
                }
            }

            traverse(visualTree, false);

            notFound = invalidationRequests.Select(e => e.Value).ToList();
        }

        public void Add(VisualTree<TKey> entity, InvalidationRequest<TKey> visualTree)
        {
            if (dict.ContainsKey(entity))
            {
                throw new EntityAlreadyInVisualTreeException($"Entity with key {entity} has already been added to the visual tree.");
            }

            dict.Add(entity, visualTree);
        }

        public IEnumerable<KeyValuePair<VisualTree<TKey>, InvalidationRequest<TKey>>> Requests() => dict;
    }
}


