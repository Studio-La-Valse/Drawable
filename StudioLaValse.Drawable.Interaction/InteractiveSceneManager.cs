using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Private;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction
{
    /// <summary>
    /// Manages an interactive scene, handling various input events and behaviors.
    /// </summary>
    /// <typeparam name="TEntity">The type of elements contained in the scene.</typeparam>
    /// <typeparam name="TKey">The type of the key used for identification.</typeparam>
    public class InteractiveSceneManager<TEntity, TKey> : SceneManager<TEntity, TKey>, IInputObserver, IObservable<BoundingBox> where TEntity : class where TKey : IEquatable<TKey>
    {
        private readonly double dragDelta = 2;
        private readonly ISelectionManager<TEntity> selectionManager;
        private readonly HashSet<IObserver<BoundingBox>> observers = [];

        private BoundingBox? lastBoundingBox;
        private XY deltaPosition;
        private XY lastMousePosition;
        private XY lastMouseClickPosition;
        private bool leftMouseIsDown;

        private bool Dragging =>
            leftMouseIsDown && lastMousePosition.DistanceTo(lastMouseClickPosition) > dragDelta;
        private bool DirectionRight =>
            lastMousePosition.X > lastMouseClickPosition.X;


        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveSceneManager{TEntity, TKey}"/> class.
        /// </summary>
        public InteractiveSceneManager(BaseVisualParent<TEntity> scene, GetKey<TEntity, TKey> getKey, BaseBitmapPainter baseBitmapPainter, ISelectionManager<TEntity> selectionManager)
            : base(scene, getKey, baseBitmapPainter)
        {
            this.selectionManager = selectionManager;
        }

        /// <summary>
        /// Travels down the visual tree and does some behavior.
        /// </summary>
        /// <param name="handleBehavior"></param>
        protected void TraverseAndHandleBehavior(Func<IBehaviorElement<TEntity>, bool> handleBehavior)
        {
            base.TraverseAndHandle(e =>
            {
                if (e is IBehaviorElement<TEntity> behavior)
                {
                    var result = handleBehavior(behavior);

                    if (!result)
                    {
                        return false;
                    }
                }

                return true;
            });
        }

        /// <inheritdoc/>
        public virtual void HandleSetMousePosition(XY position)
        {
            deltaPosition = position - lastMousePosition;

            if (!HandleMassTransform(position))
            {
                TraverseAndHandleBehavior(e => e.HandleSetMousePosition(position, RenderQueue));
            }

            HandleDragBoundingBox(position);

            RenderChanges();

            lastMousePosition = position;
        }

        /// <inheritdoc/>
        public virtual void HandleLeftMouseButtonDown()
        {
            lastMouseClickPosition = lastMousePosition;

            TraverseAndHandleBehavior(e => e.HandleLeftMouseButtonDown(RenderQueue));

            HandleInitBoundingBox();

            RenderChanges();

            leftMouseIsDown = true;
        }

        /// <inheritdoc/>
        public virtual void HandleLeftMouseButtonUp()
        {
            TraverseAndHandleBehavior(e => e.HandleLeftMouseButtonUp(RenderQueue));

            HandleHideBoundingBox();

            RenderChanges();

            leftMouseIsDown = false;
        }

        /// <inheritdoc/>
        public virtual void HandleMouseWheel(double delta)
        {
            TraverseAndHandleBehavior(e => e.HandleMouseWheel(delta, RenderQueue));

            RenderChanges();
        }

        /// <inheritdoc/>
        public virtual void HandleRightMouseButtonDown()
        {
            TraverseAndHandleBehavior(e => e.HandleRightMouseButtonDown(RenderQueue));

            RenderChanges();
        }

        /// <inheritdoc/>
        public virtual void HandleRightMouseButtonUp()
        {
            TraverseAndHandleBehavior(e => e.HandleRightMouseButtonUp(RenderQueue));

            RenderChanges();
        }

        /// <inheritdoc/>
        public virtual void HandleKeyDown(Key key)
        {
            TraverseAndHandleBehavior(e => e.HandleKeyDown(key, RenderQueue));

            RenderChanges();
        }

        /// <inheritdoc/>
        public virtual void HandleKeyUp(Key key)
        {
            TraverseAndHandleBehavior(e => e.HandleKeyUp(key, RenderQueue));

            RenderChanges();
        }

        /// <summary>
        /// Handles a mass transformation on drag.
        /// </summary>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        public virtual bool HandleMassTransform(XY newPosition)
        {
            if (!Dragging)
            {
                return false;
            }

            if (lastBoundingBox.HasValue)
            {
                return false;
            }

            foreach(var element in VisualParents.OfType<BaseTransformableParent<TEntity>>().Where(e => e.IsSelected))
            {
                if(element.Transform(deltaPosition.X, deltaPosition.Y))
                {
                    RenderQueue.Enqueue(new InvalidationRequest<TEntity>(element.AssociatedElement));
                }
            }

            return true;
        }

        /// <summary>
        /// Handles initalizing a selection border.
        /// </summary>
        public virtual void HandleInitBoundingBox()
        {
            lastBoundingBox = null;

            var elementOnPoint = VisualParents.OfType<BaseSelectableParent<TEntity>>().Where(e => e.IsSelected).Any(e => e.CaptureMouse(lastMouseClickPosition));
            if (elementOnPoint)
            {
                return;
            }

            lastBoundingBox = new BoundingBox(lastMouseClickPosition, lastMousePosition);
        }

        /// <summary>
        /// Handles updating the selection border.
        /// </summary>
        /// <param name="position"></param>
        public virtual void HandleDragBoundingBox(XY position)
        {
            if (!Dragging)
            {
                return;
            }

            if (!lastBoundingBox.HasValue)
            {
                return;
            }

            lastBoundingBox = new BoundingBox(lastMouseClickPosition, lastMousePosition);
            foreach (var observer in observers)
            {
                observer.OnNext(lastBoundingBox.Value);
            }

            var elementsInBox = VisualParents.OfType<BaseSelectableParent<TEntity>>()
                .Where(e => e.BoundingBox().Overlaps(lastBoundingBox.Value))
                .Select(e => e.OnMouseEnter())
                .OfType<InvalidationRequest<TEntity>>();
            foreach (var element in elementsInBox)
            {
                RenderQueue.Enqueue(element);
            }
        }

        /// <summary>
        /// Handles closing the selection border.
        /// </summary>
        public virtual void HandleHideBoundingBox()
        {
            if (lastBoundingBox.HasValue)
            {
                selectionManager.Clear();

                var elementsInBox = VisualParents.OfType<BaseSelectableParent<TEntity>>().Where(e => e.BoundingBox().Overlaps(lastBoundingBox.Value));
                foreach (var element in elementsInBox)
                {
                    selectionManager.Add(element.AssociatedElement);
                }

                lastBoundingBox = null;
            }

            foreach (var observer in observers)
            {
                observer.OnCompleted();
            }
        }


        /// <inheritdoc/>
        public IDisposable Subscribe(IObserver<BoundingBox> observer)
        {
            observers.Add(observer);
            return new DefaultUnsubscriber<BoundingBox>(observer, observers);
        }
    }
}