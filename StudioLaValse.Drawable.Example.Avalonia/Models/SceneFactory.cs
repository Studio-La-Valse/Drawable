using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Example.Scene;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Avalonia.Models
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
            //var scene = new TextScene(model);
            //return scene;

            var visual = new VisualGraph(model, selection, notifyEntityChanged);
            return visual;

            //var visual = new VisualCurveScene(model, selection);
            //return visual;
        }
    }
}
