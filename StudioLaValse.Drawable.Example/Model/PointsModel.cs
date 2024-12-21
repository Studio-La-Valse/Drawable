using StudioLaValse.Key;

namespace Example.Model
{
    public class PointsModel : PersistentElement, IEquatable<PointsModel>
    {
        public IEnumerable<PointModel> Points { get; }

        public PointsModel(IKeyGenerator<int> keyGenerator, INotifyEntityChanged<ElementId> notifyEntityChanged) : base(keyGenerator)
        {
            var points = new List<PointModel>();
            for (var i = 0; i < 10000; i++)
            {
                points.Add(new PointModel(keyGenerator, notifyEntityChanged));
            }

            Points = points;
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
