using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Scene
{
    public class VisualGraph : BaseVisualParent<PersistentElement>
    {
        private readonly GraphModel graph;
        private readonly ISelection<PersistentElement> selection;

        public VisualGraph(GraphModel graph, ISelection<PersistentElement> selection) : base(graph)
        {
            this.graph = graph;
            this.selection = selection;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            return graph.Components.Select(c => new VisualComponent(c, selection));
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            return new List<BaseDrawableElement>();
        }
    }
}
