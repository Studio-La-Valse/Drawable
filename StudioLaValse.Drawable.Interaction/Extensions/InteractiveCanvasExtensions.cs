using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.Private;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Extensions
{
    /// <summary>
    /// Extensions for the <see cref="IInteractiveCanvas"/>
    /// </summary>
    public static class InteractiveCanvasExtensions
    {
        /// <summary>
        /// Subscribes to an interaction <see cref="IInputObserver"/>. Returns an <see cref="IDisposable"/> that when disposed, unsubscribes from the <see cref="IInputObserver"/>
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        public static IDisposable Subscribe(this IInteractiveCanvas canvas, IInputObserver behavior)
        {
            var collection = new Queue<IDisposable>();
            var list = new List<IDisposable>()
            {
                canvas.MouseMove.Subscribe(e => behavior.HandleSetMousePosition(e)),
                canvas.MouseLeftButtonDown.Subscribe(e =>
                {
                    if (e)
                    {
                        behavior.HandleLeftMouseButtonDown();
                    }
                    else
                    {
                        behavior.HandleLeftMouseButtonUp();
                    }
                }),
                canvas.MouseRightButtonDown.Subscribe(e =>
                {
                    if (e)
                    {
                        behavior.HandleRightMouseButtonDown();
                    }
                    else
                    {
                        behavior.HandleRightMouseButtonUp();
                    }
                }),
                canvas.MouseWheel.Subscribe(e => behavior.HandleMouseWheel(e)),
                canvas.KeyDown.Subscribe(e => behavior.HandleKeyDown(e)),
                canvas.KeyUp.Subscribe(e => behavior.HandleKeyUp(e))
            };
            foreach (var element in list)
            {
                collection.Enqueue(element);
            }
            var disposable = new CollectiveDisposable(collection);
            return disposable;
        }
        /// <summary>
        /// Subscribes to a special interaction behavior that allows the <see cref="IInteractiveCanvas"/> to be panned. Unsubscribe by disposing the returned <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public static IDisposable EnablePan(this IInteractiveCanvas canvas)
        {
            var behavior = new PipeEnablePan(canvas);
            return canvas.Subscribe(behavior);
        }
        /// <summary>
        /// Subscribes to a special interaction behavior that allows the <see cref="IInteractiveCanvas"/> to be zoomed in and out. Unsubscribe by disposing the returned <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public static IDisposable EnableZoom(this IInteractiveCanvas canvas)
        {
            var behavior = new PipeEnableZoom(canvas);
            return canvas.Subscribe(behavior);
        }



        /// <summary>
        /// Transforms an <see cref="XY"/> in the viewbox space to the canvas space, taking into account the canvas' zoom and translation factors.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="pointHost"></param>
        /// <returns></returns>
        public static XY HostToCanvas(this IInteractiveCanvas canvas, XY pointHost)
        {
            var _canvasx = pointHost.X / canvas.Zoom - canvas.TranslateX;
            var _canvasy = pointHost.Y / canvas.Zoom - canvas.TranslateY;
            var pointOnCanvas = new XY(_canvasx, _canvasy);
            return pointOnCanvas;
        }
        /// <summary>
        /// Transforms an <see cref="XY"/> from canvas space to its viewbox space, taking into account the canvas' zoom and translation factors.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="pointOnCanvas"></param>
        /// <returns></returns>
        public static XY CanvasToHost(this IInteractiveCanvas canvas, XY pointOnCanvas)
        {
            var sx = (canvas.TranslateX + pointOnCanvas.X) * canvas.Zoom;
            var sy = (canvas.TranslateY + pointOnCanvas.Y) * canvas.Zoom;
            var pointOnHost = new XY(sx, sy);
            return pointOnHost;
        }



        /// <summary>
        /// Transforms a <see cref="BoundingBox"/> in the viewbox space to the canvas space, taking into account the canvas' zoom and translation factors.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="boundingBox"></param>
        /// <returns></returns>
        public static BoundingBox HostToCanvas(this IInteractiveCanvas canvas, BoundingBox boundingBox) => new(canvas.HostToCanvas(boundingBox.MinPoint), canvas.HostToCanvas(boundingBox.MaxPoint));
        /// <summary>
        /// Transforms a <see cref="BoundingBox"/> from canvas space to its viewbox space, taking into account the canvas' zoom and translation factors.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="boundingBox"></param>
        /// <returns></returns>
        public static BoundingBox CanvasToHost(this IInteractiveCanvas canvas, BoundingBox boundingBox) => new(canvas.CanvasToHost(boundingBox.MinPoint), canvas.CanvasToHost(boundingBox.MaxPoint));
    }
}
