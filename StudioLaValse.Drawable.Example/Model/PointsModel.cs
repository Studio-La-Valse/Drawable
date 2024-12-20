using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Model
{
    public class PointsModel : PersistentElement, IEquatable<PointsModel>
    {
        public IEnumerable<PointModel> Components { get; }

        public PointsModel(IKeyGenerator<int> keyGenerator, IEnumerable<PointModel> components) : base(keyGenerator)
        {
            Components = components;
        }

        public bool Equals(PointsModel? other)
        {
            if (other is null)
            {
                return false;
            }

            return other.ElementId == ElementId;
        }
    }
}
