namespace Example.Model
{
    public class CurveModel : PersistentElement
    {
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly CurveControlPoint first;
        private readonly CurveControlPoint second;
        private readonly CurveControlPoint third;
        private readonly CurveControlPoint fourth;

        public IEnumerable<CurveControlPoint> ControlPoints => [First, Second, Third, Fourth];

        public CurveControlPoint First => this.first;

        public CurveControlPoint Second => this.second;

        public CurveControlPoint Third => this.third;

        public CurveControlPoint Fourth => this.fourth;

        public CurveModel(IKeyGenerator<int> keyGenerator, INotifyEntityChanged<ElementId> notifyEntityChanged) : base(keyGenerator)
        {
            this.keyGenerator = keyGenerator;
            first = new CurveControlPoint(keyGenerator, this, notifyEntityChanged);
            second = new CurveControlPoint(keyGenerator, this, notifyEntityChanged);
            third = new CurveControlPoint(keyGenerator, this, notifyEntityChanged);
            fourth = new CurveControlPoint(keyGenerator, this, notifyEntityChanged);
        }
    }
}
