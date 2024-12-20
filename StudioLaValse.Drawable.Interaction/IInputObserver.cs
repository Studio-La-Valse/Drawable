using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction
{
    /// <summary>
    /// A single behavior in an interaction pipeline, that determines the behavior on interaction events like mouse- or key events.
    /// </summary>
    public interface IInputObserver
    {
        /// <summary>
        /// Called on mouse left button down.
        /// </summary>
        /// <returns>True if the behavior is allowed to be propagated.</returns>
        bool HandleLeftMouseButtonDown();

        /// <summary>
        /// Called on mouse left button up.
        /// </summary>
        /// <returns>True if the behavior is allowed to be propagated.</returns>
        bool HandleLeftMouseButtonUp();

        /// <summary>
        /// Called on mouse right button down.
        /// </summary>
        /// <returns>True if the behavior is allowed to be propagated.</returns>
        bool HandleRightMouseButtonDown();

        /// <summary>
        /// Called on mouse right button up.
        /// </summary>
        /// <returns>True if the behavior is allowed to be propagated.</returns>
        bool HandleRightMouseButtonUp();

        /// <summary>
        /// Called on mouse move.
        /// </summary>
        /// <returns>True if the behavior is allowed to be propagated.</returns>
        bool HandleMouseMove(XY position);

        /// <summary>
        /// Called on mouse wheel scroll.
        /// </summary>
        /// <returns>True if the behavior is allowed to be propagated.</returns>
        bool HandleMouseWheel(double delta);

        /// <summary>
        /// Called on key up.
        /// </summary>
        /// <returns>True if the behavior is allowed to be propagated.</returns>
        bool HandleKeyUp(Key key);

        /// <summary>
        /// Called on key down.
        /// </summary>
        /// <returns>True if the behavior is allowed to be propagated.</returns>
        bool HandleKeyDown(Key key);
    }
}
