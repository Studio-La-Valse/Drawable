using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;

namespace StudioLaValse.Drawable.Private
{
    internal class VisualTree<TEntity> where TEntity : class
    {
        private readonly List<VisualTree<TEntity>> childBranches = new List<VisualTree<TEntity>>();
        private readonly List<BaseDrawableElement> elements = new List<BaseDrawableElement>();
        private readonly BaseVisualParent<TEntity> visualParent;

        public TEntity Element => visualParent.AssociatedElement;
        public BaseVisualParent<TEntity> VisualParent => visualParent;
        public IEnumerable<BaseDrawableElement> Elements => elements;
        public IEnumerable<VisualTree<TEntity>> ChildBranches => childBranches;


        public VisualTree(BaseVisualParent<TEntity> visualParent)
        {
            this.visualParent = visualParent;
        }


        public void Unwrap()
        {
            elements.Clear();
            childBranches.Clear();

            var contentWrapper = VisualParent;
            Unwrap(contentWrapper);
        }

        public void Unwrap(BaseContentWrapper baseContentWrapper)
        {
            var drawableElements = baseContentWrapper.GetDrawableElements();
            foreach (var drawableElement in drawableElements)
            {
                elements.Add(drawableElement);
            }

            foreach (var _contentWrapper in baseContentWrapper.GetContentWrappers())
            {
                if (_contentWrapper is BaseVisualParent<TEntity> parent)
                {
                    var branchToDrawTo = new VisualTree<TEntity>(parent);
                    childBranches.Add(branchToDrawTo);
                    branchToDrawTo.Unwrap();
                }
                else
                {
                    Unwrap(_contentWrapper);
                }
            }
        }
    }
}
