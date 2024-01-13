using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.UserInput
{
    /// <summary>
    /// The base interface for canvas's that require interaction.
    /// </summary>
    public interface IInteractiveCanvas
    {
        /// <summary>
        /// An <see cref="IObservable{XY}"/> that emits an <see cref="XY"/> representing the current mouse position on mouse move.
        /// </summary>
        IObservable<XY> MouseMove { get; }
        /// <summary>
        /// An <see cref="IObservable{XY}"/> that emits an <see cref="bool"/> indicating whether the left mouse button is currently down.
        /// </summary>
        IObservable<bool> MouseLeftButtonDown { get; }
        /// <summary>
        /// An <see cref="IObservable{XY}"/> that emits an <see cref="bool"/> indicating whether the right mouse button is currently down.
        /// </summary>
        IObservable<bool> MouseRightButtonDown { get; }
        /// <summary>
        /// An <see cref="IObservable{XY}"/> that emits an <see cref="XY"/> representing the current mouse position on right mouse button down.
        /// </summary>
        IObservable<double> MouseWheel { get; }
        /// <summary>
        /// An <see cref="IObservable{XY}"/> that emits an <see cref="Key"/> representing the current key down.
        /// </summary>
        IObservable<Key> KeyDown { get; }
        /// <summary>
        /// An <see cref="IObservable{XY}"/> that emits an <see cref="Key"/> representing the current key up.
        /// </summary>
        IObservable<Key> KeyUp { get; }


        /// <summary>
        /// The zoomfactor of the canvas
        /// </summary>
        double Zoom { get; set; }
        /// <summary>
        /// The horizontal translation factor of the canvas
        /// </summary>
        double TranslateX { get; set; }
        /// <summary>
        /// The vertical translation factor of the canvas
        /// </summary>
        double TranslateY { get; set; }

        /// <summary>
        /// Called to redraw all cached draw requests to the target bitmap.
        /// </summary>
        void Refresh();
    }
}
