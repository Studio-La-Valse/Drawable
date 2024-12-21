using StudioLaValse.Drawable.ContentWrappers;
using Example.Model;
using Example.Scene;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Key;
using StudioLaValse.Drawable;

namespace Example.WPF.Models
{
    public class SceneFactory
    {
        private readonly ISelectionManager<PersistentElement> selection;
        private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;

        public SceneFactory(ISelectionManager<PersistentElement> selection, INotifyEntityChanged<ElementId> notifyEntityChanged)
        {
            this.selection = selection;
            this.notifyEntityChanged = notifyEntityChanged;
        }


        public BaseVisualParent<ElementId> Create(PointsModel model)
        {
            var visual = new VisualPoints(model, selection, notifyEntityChanged);
            return visual;
        }
    }
}
