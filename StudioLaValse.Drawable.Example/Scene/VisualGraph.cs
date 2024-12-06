using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Interaction;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Scene
{
    public class VisualGraph : BaseVisualParent<PersistentElement>
    {
        private readonly GraphModel graph;
        private readonly ISelectionManager<PersistentElement> selection;

        public VisualGraph(GraphModel graph, ISelectionManager<PersistentElement> selection) : base(graph)
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

        public override BoundingBox BoundingBox()
        {
            return new BoundingBox(GetContentWrappers().Select(e => e.BoundingBox()));
        }
    }
}
