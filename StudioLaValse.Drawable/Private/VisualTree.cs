using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;

namespace StudioLaValse.Drawable.Private
{
    internal class VisualTree<TEntity> where TEntity : class
    {
        private readonly List<VisualTree<TEntity>> childBranches = [];
        private readonly List<BaseDrawableElement> elements = [];
        private readonly List<BaseContentWrapper> contentWrappers = [];
        private readonly BaseVisualParent<TEntity> visualParent;

        public TEntity Element => visualParent.AssociatedElement;
        public BaseVisualParent<TEntity> VisualParent => visualParent;
        public IEnumerable<BaseDrawableElement> Elements => elements;
        public IEnumerable<VisualTree<TEntity>> ChildBranches => childBranches;


        public VisualTree(BaseVisualParent<TEntity> visualParent)
        {
            this.visualParent = visualParent;
            this.contentWrappers.Add(visualParent);
        }


        public void Regenerate()
        {
            elements.Clear();
            contentWrappers.Clear();
            childBranches.Clear();

            var contentWrapper = VisualParent;
            contentWrappers.Add(contentWrapper);
            Regenerate(contentWrapper);
        }
        private void Regenerate(BaseContentWrapper baseContentWrapper)
        {
            var drawableElements = baseContentWrapper.GetDrawableElements();
            foreach (var drawableElement in drawableElements)
            {
                elements.Add(drawableElement);
            }

            foreach (var _contentWrapper in baseContentWrapper.GetContentWrappers())
            {
                contentWrappers.Add(_contentWrapper);

                if (_contentWrapper is BaseVisualParent<TEntity> parent)
                {
                    var branchToDrawTo = new VisualTree<TEntity>(parent);
                    childBranches.Add(branchToDrawTo);
                    branchToDrawTo.Regenerate();
                }
                else
                {
                    Regenerate(_contentWrapper);
                }
            }
        }


        public void Rebuild()
        {
            elements.Clear();

            foreach (var contentWrapper in contentWrappers)
            {
                var drawableElements = contentWrapper.GetDrawableElements();
                foreach (var drawableElement in drawableElements)
                {
                    elements.Add(drawableElement);
                }
            }

            foreach (var child in childBranches)
            {
                child.Rebuild();
            }
        }



        public void Redraw()
        {
            elements.Clear();

            foreach (var contentWrapper in contentWrappers)
            {
                var drawableElements = contentWrapper.GetDrawableElements();
                foreach (var drawableElement in drawableElements)
                {
                    elements.Add(drawableElement);
                }
            }
        }
    }
}
