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
    public class VisualPoints : BaseVisualParent<ElementId>
    {
        private readonly PointsModel graph;
        private readonly ISelectionManager<PersistentElement> selection;
        private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;

        public VisualPoints(PointsModel graph, ISelectionManager<PersistentElement> selection, INotifyEntityChanged<ElementId> notifyEntityChanged) : base(graph.ElementId)
        {
            this.graph = graph;
            this.selection = selection;
            this.notifyEntityChanged = notifyEntityChanged;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            return graph.Components.Select(c => new VisualPoint(c, selection, notifyEntityChanged));
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
