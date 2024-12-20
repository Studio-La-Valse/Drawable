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
    public class InteractiveSceneManager<TKey> : SceneManager<TKey>, IInputObserver where TKey : IEquatable<TKey>
    {
        private readonly double dragDelta = 2;

        private BoundingBox? lastBoundingBox;
        private XY deltaPosition;
        private XY lastMousePosition;
        private XY lastMouseDownPosition;
        private bool leftMouseIsDown;

        private bool Dragging =>
            leftMouseIsDown && lastMousePosition.DistanceTo(lastMouseDownPosition) > dragDelta;
        private bool DirectionRight =>
            lastMousePosition.X > lastMouseDownPosition.X;


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
        protected bool TraverseAndHandleBehavior(Func<IInputObserver, bool> handleBehavior)
        {
            return base.TraverseAndHandle(e =>
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
        public virtual bool HandleMouseMove(XY position)
        {
            deltaPosition = position - lastMousePosition;

            lastMousePosition = position;

            if (HandleDragBoundingBox())
            {
                if (HandleMassTransform())
                {
                    TraverseAndHandleBehavior(e => e.HandleMouseMove(position));
                }
            }

            RenderChanges();

            return true;
        }

        /// <inheritdoc/>
        public virtual bool HandleLeftMouseButtonDown()
        {
            lastMouseDownPosition = lastMousePosition;

            if (TraverseAndHandleBehavior(e => e.HandleLeftMouseButtonDown()))
            {
                HandleInitBoundingBox();
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
                    var _continue = transformable.HandleMouseMove(lastMousePosition);

                    if (transformable.IsSelected)
                    {
                        transformable.Transform(deltaPosition.X, deltaPosition.Y);
                    }

                    return _continue;
                }
                else if (e is BaseInteractiveParent<TKey> interactive)
                {
                    return interactive.HandleMouseMove(lastMousePosition);
                }
                return true;
            });

            return false;
        }

        private SelectionBorder? _selectionBorder = null;
        /// <summary>
        /// Add a selection border to the scene manager to allow a drag selection border.
        /// </summary>
        /// <param name="selectionBorder"></param>
        /// <returns></returns>
        public InteractiveSceneManager<TKey> AddSelectionBorder(SelectionBorder selectionBorder)
        {
            _selectionBorder = selectionBorder;
            return this;
        }

        /// <summary>
        /// Handles initalizing a selection border.
        /// </summary>
        public virtual bool HandleInitBoundingBox()
        {
            if(_selectionBorder is null)
            {
                return true;
            }

            lastBoundingBox = null;

            var elementOnPoint = false;
            TraverseAndHandle(e =>
            {
                if (elementOnPoint)
                {
                    return false;
                }

                if (e is BaseSelectableParent<TKey> selectable && selectable.IsSelected && selectable.CaptureMouse(lastMouseDownPosition))
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

            lastBoundingBox = new BoundingBox(lastMouseDownPosition, lastMousePosition);

            return true;
        }

        /// <summary>
        /// Handles updating the selection border.
        /// </summary>
        public virtual bool HandleDragBoundingBox()
        {
            if (_selectionBorder is null)
            {
                return true;
            }

            if (!Dragging)
            {
                return true;
            }

            if (!lastBoundingBox.HasValue)
            {
                return true;
            }

            lastBoundingBox = new BoundingBox(lastMouseDownPosition, lastMousePosition);
            _selectionBorder.Set(lastBoundingBox.Value);

            TraverseAndHandle(e =>
            {
                if (e is BaseSelectableParent<TKey> selectable)
                {
                    var overlap = DirectionRight ?
                        lastBoundingBox.Value.Contains(selectable.BoundingBox()) :
                        selectable.BoundingBox().Overlaps(lastBoundingBox.Value);
                    if (overlap)
                    {
                        selectable.OnMouseEnter();
                        return false;
                    }
                    else
                    {
                        selectable.OnMouseLeave();
                    }
                    return true;
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
            if (_selectionBorder is null)
            {
                return true;
            }

            if (Dragging && lastBoundingBox.HasValue)
            {
                TraverseAndHandle(e =>
                {
                    if (e is BaseSelectableParent<TKey> selectable)
                    {
                        var select = DirectionRight ?
                            lastBoundingBox.Value.Contains(selectable.BoundingBox()) :
                            selectable.BoundingBox().Overlaps(lastBoundingBox.Value);
                        if(select)
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
            }

            lastBoundingBox = null;
            _selectionBorder.Hide();

            return true;
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

    /// <summary>
    /// A selection border that may be observed.
    /// </summary>
    public class SelectionBorder : IObservable<BoundingBox>
    {
        private readonly HashSet<IObserver<BoundingBox>> observers = [];

        /// <inheritdoc/>
        public IDisposable Subscribe(IObserver<BoundingBox> observer)
        {
            observers.Add(observer);
            return new DefaultUnsubscriber<BoundingBox>(observer, observers);
        }
        
        /// <summary>
        /// Set the specified bounding box to be the next selection border.
        /// </summary>
        /// <param name="boundingBox"></param>
        public void Set(BoundingBox boundingBox)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(boundingBox);
            }
        }

        /// <summary>
        /// Hide the selection border. 
        /// </summary>
        public void Hide()
        {
            foreach (var observer in observers)
            {
                observer.OnCompleted();
            }
        }
    }
}

