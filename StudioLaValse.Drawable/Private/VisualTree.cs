using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StudioLaValse.Drawable.Private
{
    /// <summary>
    /// Represents a visual tree containing elements of type <typeparamref name="TKey"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of elements contained in the visual tree.</typeparam>
    internal class VisualTree<TKey> where TKey : IEquatable<TKey>
    {
        private readonly List<VisualTree<TKey>> childBranches = new List<VisualTree<TKey>>();
        private readonly List<BaseDrawableElement> elements = new List<BaseDrawableElement>();
        private readonly List<BaseContentWrapper> contentWrappers = new List<BaseContentWrapper>();
        private readonly BaseVisualParent<TKey> visualParent;

        /// <summary>
        /// Gets the key associated with the visual parent.
        /// </summary>
        public TKey Key => visualParent.Key;

        /// <summary>
        /// Gets the visual parent.
        /// </summary>
        public BaseVisualParent<TKey> VisualParent => visualParent;

        /// <summary>
        /// Gets the drawable elements in the visual tree.
        /// </summary>
        public IEnumerable<BaseDrawableElement> Elements => elements;

        /// <summary>
        /// Gets the child branches of the visual tree.
        /// </summary>
        public IEnumerable<VisualTree<TKey>> ChildBranches => childBranches;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualTree{TEntity}"/> class.
        /// </summary>
        /// <param name="visualParent">The visual parent associated with the tree.</param>
        public VisualTree(BaseVisualParent<TKey> visualParent)
        {
            this.visualParent = visualParent;
            contentWrappers.Add(visualParent);
        }

        /// <summary>
        /// Regenerates the visual tree, clearing and repopulating elements, content wrappers, and child branches.
        /// </summary>
        public void Regenerate()
        {
            elements.Clear();
            contentWrappers.Clear();
            childBranches.Clear();

            var contentWrapper = VisualParent;
            contentWrappers.Add(contentWrapper);
            Regenerate(contentWrapper);
        }

        /// <summary>
        /// Helper method to recursively regenerate the visual tree.
        /// </summary>
        /// <param name="baseContentWrapper">The base content wrapper to regenerate from.</param>
        public void Regenerate(BaseContentWrapper baseContentWrapper)
        {
            var drawableElements = baseContentWrapper.GetDrawableElements();
            foreach (var drawableElement in drawableElements)
            {
                elements.Add(drawableElement);
            }

            foreach (var _contentWrapper in baseContentWrapper.GetContentWrappers())
            {
                contentWrappers.Add(_contentWrapper);

                if (_contentWrapper is BaseVisualParent<TKey> parent)
                {
                    var branchToDrawTo = new VisualTree<TKey>(parent);
                    childBranches.Add(branchToDrawTo);
                    branchToDrawTo.Regenerate();
                }
                else
                {
                    Regenerate(_contentWrapper);
                }
            }
        }

        /// <summary>
        /// Rebuilds the visual tree, clearing and repopulating the drawable elements.
        /// </summary>
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

        /// <summary>
        /// Redraws the visual tree, clearing and repopulating the drawable elements.
        /// </summary>
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

        /// <summary>
        /// Traverses the visual tree and handles behavior based on the provided function.
        /// </summary>
        /// <param name="handleBehavior">The function to handle the behavior for each node in the tree.</param>
        public void TraverseAndHandle(Func<BaseVisualParent<TKey>, bool> handleBehavior)
        {
            var result = handleBehavior(VisualParent);

            if (!result)
            {
                return;
            }

            foreach (var child in ChildBranches)
            {
                child.TraverseAndHandle(handleBehavior);
            }
        }
    }
}
