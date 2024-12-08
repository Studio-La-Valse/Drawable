using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class PipeEnableZoom : IInputObserver
    {
        private readonly IInteractiveCanvas canvas;

        public XY MouseLocation { get; set; } = new XY(0, 0);

        public PipeEnableZoom(IInteractiveCanvas canvas)
        {
            this.canvas = canvas;
        }

        public bool HandleLeftMouseButtonDown()
        {
            return true;
        }

        public bool HandleLeftMouseButtonUp()
        {
            return true;
        }

        public bool HandleMouseWheel(double delta)
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

            return true;
        }

        public bool HandleRightMouseButtonDown()
        {
            return true;
        }

        public bool HandleRightMouseButtonUp()
        {
            return true;
        }

        public bool HandleSetMousePosition(XY position)
        {
            var positionOnHost = canvas.CanvasToHost(position);
            MouseLocation = positionOnHost;
            return true;
        }

        public bool HandleKeyDown(Key key)
        {
            return true;
        }

        public bool HandleKeyUp(Key key)
        {
            return true;
        }
    }
}
