using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Scene
{
    public class VisualCurveScene : BaseVisualParent<PersistentElement>
    {
        private readonly CurveScene curveScene;
        private readonly ISelection<PersistentElement> selection;

        public VisualCurveScene(CurveScene curveScene, ISelection<PersistentElement> selection) : base(curveScene)
        {
            this.curveScene = curveScene;
            this.selection = selection;
        }

        public override IEnumerable<BaseContentWrapper> GetContentWrappers()
        {
            return new List<BaseContentWrapper>()
            {
                new VisualCurve(curveScene.CurveModel, selection)
            };
        }

        public override IEnumerable<BaseDrawableElement> GetDrawableElements()
        {
            return new List<BaseDrawableElement>()
            {

            };
        }
    }
}
