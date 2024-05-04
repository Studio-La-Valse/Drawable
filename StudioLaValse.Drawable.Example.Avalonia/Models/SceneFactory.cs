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

        public SceneFactory(ISelectionManager<PersistentElement> selection)
        {
            this.selection = selection;
        }


        public BaseVisualParent<PersistentElement> Create(GraphModel model)
        {
            var visual = new VisualGraph(model, selection);
            return visual;
        }
    }
}
