using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.UserInput
{
    /// <summary>
    /// A single pipe in an interaction pipeline, that determines the behavior on interaction events like mouse- or key events.
    /// </summary>
    public interface IPipe
    {
        /// <summary>
        /// Called on mouse left button down.
        /// </summary>
        void HandleLeftMouseButtonDown();
        /// <summary>
        /// Called on mouse left button up.
        /// </summary>
        void HandleLeftMouseButtonUp();
        /// <summary>
        /// Called on mouse right button down.
        /// </summary>
        void HandleRightMouseButtonDown();
        /// <summary>
        /// Called on mouse right button up.
        /// </summary>
        void HandleRightMouseButtonUp();
        /// <summary>
        /// Called on mouse move.
        /// </summary>
        void HandleSetMousePosition(XY position);
        /// <summary>
        /// Called on mouse wheel scroll.
        /// </summary>
        void HandleMouseWheel(double delta);
        /// <summary>
        /// Called on key up.
        /// </summary>
        void KeyUp(Key key);
        /// <summary>
        /// Called on key down.
        /// </summary>
        void KeyDown(Key key);
    }
}
