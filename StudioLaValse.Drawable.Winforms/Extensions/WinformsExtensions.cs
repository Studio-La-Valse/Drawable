using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Winforms.Extensions
{
    public static class WinformsExtensions
    {
        public static Color ToWindowsColor(this ColorARGB color)
        {
            var clr = Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
            return clr;
        }

        public static Color ToWindowsColor(this ColorRGB color)
        {
            var clr = Color.FromArgb(color.Red, color.Green, color.Blue);
            return clr;
        }


        public static Brush ToWindowsBrush(this ColorARGB color)
        {
            var clr = color.ToWindowsColor();
            var brush = new SolidBrush(clr);
            return brush;
        }

        public static Point ToWindowsPoint(this XY point)
        {
            var pnt = new Point((int)point.X, (int)point.Y);
            return pnt;
        }

        public static XY FromWindowsPoint(this Point point)
        {
            var xy = new XY(point.X, point.Y);
            return xy;
        }
    }

}