using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Example.Scene;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System;
using System.Linq;

namespace StudioLaValse.Drawable.Example.Avalonia.Models
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
            //var text = new TextModel(keyGenerator);
            //return text;

            var n = 5000;
            var components = Enumerable.Range(0, n).Select(i => new ComponentModel(keyGenerator, new BaseGhost(keyGenerator), notifyEntityChanged)).ToArray();
            var graph = new GraphModel(keyGenerator, components);
            return graph;

            //var curve = new CurveModel(keyGenerator);

            //var scene = new CurveScene(keyGenerator, curve);
            //return scene;
        }
    }
}
