using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction
{
    /// <summary>
    /// Defines an interface that allows implementation of behavior as a response to user input.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBehaviorElement<TEntity>
    {
        /// <summary>
        /// Called on mouse left button down.
        /// </summary>
        /// <param name="invalidationRequests">The queue of elements that will be invalidated after the bevahior has propagated through all children.</param>
        /// <returns>True when the behavior is allowed to propagate throught its children.</returns>
        bool HandleLeftMouseButtonDown(Queue<InvalidationRequest<TEntity>> invalidationRequests);
        /// <summary>
        /// Called on mouse left button up.
        /// </summary>
        /// <param name="invalidationRequests">The queue of elements that will be invalidated after the bevahior has propagated through all children.</param>
        /// <returns>True when the behavior is allowed to propagate throught its children.</returns>
        bool HandleLeftMouseButtonUp(Queue<InvalidationRequest<TEntity>> invalidationRequests);
        /// <summary>
        /// Called on mouse right button down.
        /// </summary>
        /// <param name="invalidationRequests">The queue of elements that will be invalidated after the bevahior has propagated through all children.</param>
        /// <returns>True when the behavior is allowed to propagate throught its children.</returns>
        bool HandleRightMouseButtonDown(Queue<InvalidationRequest<TEntity>> invalidationRequests);
        /// <summary>
        /// Called on mouse right button up.
        /// </summary>
        /// <param name="invalidationRequests">The queue of elements that will be invalidated after the bevahior has propagated through all children.</param>
        /// <returns>True when the behavior is allowed to propagate throught its children.</returns>
        bool HandleRightMouseButtonUp(Queue<InvalidationRequest<TEntity>> invalidationRequests);
        /// <summary>
        /// Called on mouse move.
        /// </summary>
        /// <param name="position">The captured mouse position.</param>
        /// <param name="invalidationRequests">The queue of elements that will be invalidated after the bevahior has propagated through all children.</param>
        /// <returns>True when the behavior is allowed to propagate throught its children.</returns>
        bool HandleSetMousePosition(XY position, Queue<InvalidationRequest<TEntity>> invalidationRequests);
        /// <summary>
        /// Called on mouse wheel scroll.
        /// </summary>
        /// <param name="delta">The mousewheel delta.</param>
        /// <param name="invalidationRequests">The queue of elements that will be invalidated after the bevahior has propagated through all children.</param>
        /// <returns>True when the behavior is allowed to propagate throught its children.</returns>
        bool HandleMouseWheel(double delta, Queue<InvalidationRequest<TEntity>> invalidationRequests);
        /// <summary>
        /// Called on key up.
        /// </summary>
        /// <param name="key">The released key.</param>
        /// <param name="invalidationRequests">The queue of elements that will be invalidated after the bevahior has propagated through all children.</param>
        /// <returns>True when the behavior is allowed to propagate throught its children.</returns>
        bool HandleKeyUp(Key key, Queue<InvalidationRequest<TEntity>> invalidationRequests);
        /// <summary>
        /// Called on key down.
        /// </summary>
        /// <param name="key">The pressed key.</param>
        /// <param name="invalidationRequests">The queue of elements that will be invalidated after the bevahior has propagated through all children.</param>
        /// <returns>True when the behavior is allowed to propagate throught its children.</returns>
        bool HandleKeyDown(Key key, Queue<InvalidationRequest<TEntity>> invalidationRequests);
    }
}