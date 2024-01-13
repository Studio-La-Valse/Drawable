using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class PipeEnableZoom : IPipe
    {
        private readonly IInteractiveCanvas canvas;

        public XY MouseLocation { get; set; } = new XY(0, 0);

        public PipeEnableZoom(IInteractiveCanvas canvas)
        {
            this.canvas = canvas;
        }

        public void HandleLeftMouseButtonDown()
        {

        }

        public void HandleLeftMouseButtonUp()
        {

        }

        public void HandleMouseWheel(double delta)
        {
            //store mouse position in canvas coordinate space
            var mouseOnCanvas = canvas.HostToCanvas(MouseLocation);

            //do actual zooming
            canvas.Zoom *= 1 + delta;

            //transform the mouse position back to screen space, add delta to translatex and ~y
            var mouseOnScreen = canvas.CanvasToHost(mouseOnCanvas);
            canvas.TranslateX += (MouseLocation.X - mouseOnScreen.X) / canvas.Zoom;
            canvas.TranslateY += (MouseLocation.Y - mouseOnScreen.Y) / canvas.Zoom;

            canvas.Refresh();
        }

        public void HandleRightMouseButtonDown()
        {

        }

        public void HandleRightMouseButtonUp()
        {

        }

        public void HandleSetMousePosition(XY position)
        {
            var positionOnHost = canvas.CanvasToHost(position);
            MouseLocation = positionOnHost;
        }

        public void KeyDown(Key key)
        {

        }

        public void KeyUp(Key key)
        {

        }
    }
}
