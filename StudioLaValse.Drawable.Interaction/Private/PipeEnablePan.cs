using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class PipeEnablePan : IPipe
    {
        private readonly IInteractiveCanvas canvas;

        public bool RightButtonDown { get; set; } = false;
        public XY LastPosition { get; set; } = new XY(0, 0);

        public PipeEnablePan(IInteractiveCanvas interactiveCanvas)
        {
            canvas = interactiveCanvas;
        }

        public void HandleLeftMouseButtonDown()
        {

        }

        public void HandleLeftMouseButtonUp()
        {

        }

        public void HandleMouseWheel(double delta)
        {

        }

        public void HandleRightMouseButtonDown()
        {
            RightButtonDown = true;
        }

        public void HandleRightMouseButtonUp()
        {
            RightButtonDown = false;
        }

        public void HandleSetMousePosition(XY position)
        {
            var positionOnHost = canvas.CanvasToHost(position);

            if (positionOnHost.DistanceTo(LastPosition) < 0.01)
            {
                return;
            }

            var dx = positionOnHost.X - LastPosition.X;
            var dy = positionOnHost.Y - LastPosition.Y;

            if (RightButtonDown)
            {
                canvas.TranslateX += dx / canvas.Zoom;
                canvas.TranslateY += dy / canvas.Zoom;
                canvas.Refresh();
            }

            LastPosition = positionOnHost;
        }

        public void KeyDown(Key key)
        {

        }

        public void KeyUp(Key key)
        {

        }
    }
}
