using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Example.Scene;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.WPF.Models
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


        public BaseVisualParent<ElementId> Create(GraphModel model)
        {
            var visual = new VisualGraph(model, selection, notifyEntityChanged);
            return visual;
        }
    }
}
