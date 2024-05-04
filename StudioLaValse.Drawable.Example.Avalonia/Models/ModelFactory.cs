using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Key;
using System.Linq;

namespace StudioLaValse.Drawable.Example.Avalonia.Models
{
    public class ModelFactory
    {
        private readonly IKeyGenerator<int> keyGenerator;

        public ModelFactory(IKeyGenerator<int> keyGenerator)
        {
            this.keyGenerator = keyGenerator;
        }

        public GraphModel Create()
        {
            var n = 5000;
            var components = Enumerable.Range(0, n).Select(i => new ComponentModel(keyGenerator, new BaseGhost(keyGenerator))).ToArray();
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
