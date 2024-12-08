using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class PipeEnablePan : IInputObserver
    {
        private readonly IInteractiveCanvas canvas;

        public bool RightButtonDown { get; set; } = false;
        public XY LastPosition { get; set; } = new XY(0, 0);

        public PipeEnablePan(IInteractiveCanvas interactiveCanvas)
        {
            canvas = interactiveCanvas;
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
            return true;
        }

        public bool HandleRightMouseButtonDown()
        {
            RightButtonDown = true;
            return true;
        }

        public bool HandleRightMouseButtonUp()
        {
            RightButtonDown = false;
            return true;
        }

        public bool HandleSetMousePosition(XY position)
        {
            var positionOnHost = canvas.CanvasToHost(position);

            if (positionOnHost.DistanceTo(LastPosition) < 0.01)
            {
                return true;
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
