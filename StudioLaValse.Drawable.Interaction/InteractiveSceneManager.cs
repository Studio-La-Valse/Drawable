using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Private;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using System.Xml.Linq;

namespace StudioLaValse.Drawable.Interaction
{
    /// <summary>
    /// Manages an interactive scene, handling various input events and behaviors.
    /// </summary>
    /// <typeparam name="TKey">The type of the key used for identification.</typeparam>
    public class InteractiveSceneManager<TKey> : SceneManager<TKey>, IInputObserver, IObservable<BoundingBox> where TKey : IEquatable<TKey>
    {
        private readonly double dragDelta = 2;
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
        /// Initializes a new instance of the <see cref="InteractiveSceneManager{TKey}"/> class.
        /// </summary>
        public InteractiveSceneManager(BaseVisualParent<TKey> scene, BaseBitmapPainter baseBitmapPainter) : base(scene, baseBitmapPainter)
        {

        }

        /// <summary>
        /// Travels down the visual tree and does some behavior.
        /// </summary>
        /// <param name="handleBehavior"></param>
        protected void TraverseAndHandleBehavior(Func<IInputObserver, bool> handleBehavior)
        {
            base.TraverseAndHandle(e =>
            {
                if (e is IInputObserver behavior)
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
        public virtual bool HandleSetMousePosition(XY position)
        {
            deltaPosition = position - lastMousePosition;

            lastMousePosition = position;

            if (HandleDragBoundingBox())
            {
                if (HandleMassTransform())
                {
                    TraverseAndHandleBehavior(e => e.HandleSetMousePosition(position));
                }
            }

            RenderChanges();

            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleLeftMouseButtonDown()
        {
            lastMouseClickPosition = lastMousePosition;

            if (HandleInitBoundingBox())
            {
                TraverseAndHandleBehavior(e => e.HandleLeftMouseButtonDown());
            }

            RenderChanges();

            leftMouseIsDown = true;

            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleLeftMouseButtonUp()
        {
            if (HandleHideBoundingBox())
            {
                TraverseAndHandleBehavior(e => e.HandleLeftMouseButtonUp());
            }

            RenderChanges();

            leftMouseIsDown = false;

            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleMouseWheel(double delta)
        {
            TraverseAndHandleBehavior(e => e.HandleMouseWheel(delta));

            RenderChanges();

            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleRightMouseButtonDown()
        {
            TraverseAndHandleBehavior(e => e.HandleRightMouseButtonDown());

            RenderChanges();

            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleRightMouseButtonUp()
        {
            TraverseAndHandleBehavior(e => e.HandleRightMouseButtonUp());

            RenderChanges();

            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleKeyDown(Key key)
        {
            TraverseAndHandleBehavior(e => e.HandleKeyDown(key));

            RenderChanges();

            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleKeyUp(Key key)
        {
            TraverseAndHandleBehavior(e => e.HandleKeyUp(key));

            RenderChanges();

            return true;
        }

        /// <summary>
        /// Handles a mass transformation on drag.
        /// </summary>
        public virtual bool HandleMassTransform()
        {
            if (!Dragging)
            {
                return true;
            }

            TraverseAndHandleBehavior(e =>
            {
                if(e is BaseTransformableParent<TKey> transformable)
                {
                    using var lockTransform = new LockTransform(transformable);
                    var _continue = transformable.HandleSetMousePosition(lastMousePosition);

                    if (transformable.IsSelected)
                    {
                        transformable.Transform(deltaPosition.X, deltaPosition.Y);
                    }

                    return _continue;
                }
                else if (e is BaseInteractiveParent<TKey> interactive)
                {
                    return interactive.HandleSetMousePosition(lastMousePosition);
                }
                return true;
            });

            return false;
        }

        /// <summary>
        /// Handles initalizing a selection border.
        /// </summary>
        public virtual bool HandleInitBoundingBox()
        {
            lastBoundingBox = null;

            var elementOnPoint = false;
            TraverseAndHandle(e =>
            {
                if (elementOnPoint)
                {
                    return false;
                }

                if (e is BaseSelectableParent<TKey> selectable && selectable.IsSelected && selectable.CaptureMouse(lastMouseClickPosition))
                {
                    elementOnPoint = true;
                    return false;
                }

                return true;
            });
            if (elementOnPoint)
            {
                return true;
            }

            lastBoundingBox = new BoundingBox(lastMouseClickPosition, lastMousePosition);

            return true;
        }

        /// <summary>
        /// Handles updating the selection border.
        /// </summary>
        public virtual bool HandleDragBoundingBox()
        {
            if (!Dragging)
            {
                return true;
            }

            if (!lastBoundingBox.HasValue)
            {
                return true;
            }

            lastBoundingBox = new BoundingBox(lastMouseClickPosition, lastMousePosition);
            foreach (var observer in observers)
            {
                observer.OnNext(lastBoundingBox.Value);
            }

            TraverseAndHandle(e =>
            {
                if (e is BaseInteractiveParent<TKey> interactive)
                {
                    var _continue = interactive.HandleSetMousePosition(lastMousePosition);
                    if (e is BaseSelectableParent<TKey> selectable && selectable.BoundingBox().Overlaps(lastBoundingBox.Value))
                    {
                        selectable.OnMouseEnter();
                    }

                    return _continue;
                }
                
                return true;
            });

            return false;
        }

        /// <summary>
        /// Handles closing the selection border.
        /// </summary>
        public virtual bool HandleHideBoundingBox()
        {
            if (Dragging && lastBoundingBox.HasValue)
            {
                TraverseAndHandle(e =>
                {
                    if (e is BaseSelectableParent<TKey> selectable)
                    {
                        if(selectable.BoundingBox().Overlaps(lastBoundingBox.Value))
                        { 
                            selectable.Select(); 
                        }
                        else
                        {
                            selectable.Deselect();
                        }
                    }
                    return true;
                });

                lastBoundingBox = null;
            }

            foreach (var observer in observers)
            {
                observer.OnCompleted();
            }

            return true;
        }


        /// <inheritdoc/>
        public IDisposable Subscribe(IObserver<BoundingBox> observer)
        {
            observers.Add(observer);
            return new DefaultUnsubscriber<BoundingBox>(observer, observers);
        }

        class LockTransform : IDisposable
        {
            private readonly BaseTransformableParent<TKey> baseTransformableParent;

            public LockTransform(BaseTransformableParent<TKey> baseTransformableParent)
            {
                this.baseTransformableParent = baseTransformableParent;
                baseTransformableParent.LockTransform = true;
            }
            public void Dispose()
            {
                baseTransformableParent.LockTransform = false;
            }
        }
    }
}

