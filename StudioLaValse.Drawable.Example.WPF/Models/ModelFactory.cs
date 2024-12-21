using Example.Model;
using StudioLaValse.Drawable;
using StudioLaValse.Key;
using System.Linq;

namespace Example.WPF.Models
{
    public class ModelFactory
    {
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;

        public ModelFactory(IKeyGenerator<int> keyGenerator, INotifyEntityChanged<ElementId> notifyEntityChanged)
        {
            this.keyGenerator = keyGenerator;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public PointsModel Create()
        {
            // return new TextModel(keyGenerator);

            var graph = new PointsModel(keyGenerator, notifyEntityChanged);
            return graph;

            //var curve = new CurveModel(keyGenerator);
            //foreach(var _ in Enumerable.Range(0, 4))
            //{
            //    var point = new XY(Random.Shared.Next() % n, Random.Shared.Next() % n);
            //    curve.AddControlPoint(point);
            //}

            //var scene = new CurveScene(keyGenerator, curve);
            //var visualScene = new VisualCurveScene(scene, selection);
        }
    }
}
