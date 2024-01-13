using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using System.Drawing.Drawing2D;

namespace StudioLaValse.Drawable.Winforms.Controls
{
    public class ControlContainer : BaseInteractiveControl, ISelectionBorder
    {
        public bool SelectionBorderVisible { get; set; } = false;
        public BoundingBox SelectionBorder { get; set; } = new BoundingBox(0, 0, 0, 0);

        public List<Action<Graphics>> DrawActions { get; } = new List<Action<Graphics>>();

        public ControlContainer()
        {
            Dock = DockStyle.Fill;
            DoubleBuffered = true;

            Paint += ControlContainer_Paint;
        }


        public void SetVisibility(bool visible)
        {
            SelectionBorderVisible = visible;
        }

        public void Set(BoundingBox boundingBox)
        {
            SelectionBorder = boundingBox;
        }



        private void ControlContainer_Paint(object? sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.ScaleTransform((float)Zoom, (float)Zoom);
            graphics.TranslateTransform((float)TranslateX, (float)TranslateY);

            foreach (var action in DrawActions)
            {
                action(graphics);
            }

            if (SelectionBorderVisible)
            {
                var color = Color.FromKnownColor(KnownColor.SkyBlue);
                var brush = new SolidBrush(Color.FromArgb(100, color));
                var rect = new Rectangle((int)SelectionBorder.MinPoint.X, (int)SelectionBorder.MinPoint.Y, (int)SelectionBorder.Width, (int)SelectionBorder.Height);
                var pen = new Pen(color);
                graphics.FillRectangle(brush, rect);
                graphics.DrawRectangle(pen, rect);
            }
        }


        public override void Refresh()
        {
            Invalidate();
        }
    }
}
