using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Key;
using System.Linq;

namespace StudioLaValse.Drawable.Example.WPF.Models
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

        public GraphModel Create()
        {
            // return new TextModel(keyGenerator);

            var n = 5000;
            var components = Enumerable.Range(0, n).Select(i => new ComponentModel(keyGenerator, new BaseGhost(keyGenerator), notifyEntityChanged)).ToArray();
            var graph = new GraphModel(keyGenerator, components);
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
