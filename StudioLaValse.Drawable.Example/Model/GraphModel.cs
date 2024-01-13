using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Model
{
    public class GraphModel : PersistentElement, IEquatable<GraphModel>
    {
        public IEnumerable<ComponentModel> Components { get; }

        public GraphModel(IKeyGenerator<int> keyGenerator, IEnumerable<ComponentModel> components) : base(keyGenerator)
        {
            Components = components;
        }

        public bool Equals(GraphModel? other)
        {
            if (other is null)
            {
                return false;
            }

            return other.ElementId == ElementId;
        }
    }
}
