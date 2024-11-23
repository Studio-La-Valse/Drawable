using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class PipeInterceptKeys<TEntity> : IBehavior where TEntity : class
    {
        private readonly IBehavior source;

        public bool ShiftPressed { get; set; }
        public bool CtrlPressed { get; set; }

        public PipeInterceptKeys(IBehavior source)
        {
            this.source = source;
        }

        public void HandleLeftMouseButtonDown()
        {
            source.HandleLeftMouseButtonDown();
        }

        public void HandleLeftMouseButtonUp()
        {
            source.HandleLeftMouseButtonUp();
        }

        public void HandleMouseWheel(double delta)
        {
            source.HandleMouseWheel(delta);
        }

        public void HandleRightMouseButtonDown()
        {
            source.HandleRightMouseButtonDown();
        }

        public void HandleRightMouseButtonUp()
        {
            source.HandleRightMouseButtonUp();
        }

        public void HandleSetMousePosition(XY position)
        {
            source.HandleSetMousePosition(position);
        }

        public void KeyDown(Key key)
        {
            if (key == Key.Shift)
            {
                ShiftPressed = true;
            }

            if (key == Key.Control)
            {
                CtrlPressed = true;
            }

            source.KeyDown(key);
        }

        public void KeyUp(Key key)
        {
            if (key == Key.Shift)
            {
                ShiftPressed = false;
            }

            if (key == Key.Control)
            {
                CtrlPressed = false;
            }

            source.KeyUp(key);
        }
    }
}
