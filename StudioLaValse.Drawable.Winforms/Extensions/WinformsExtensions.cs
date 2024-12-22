using StudioLaValse.Drawable.Text;
using StudioLaValse.Geometry;
using static System.Net.Mime.MediaTypeNames;

namespace StudioLaValse.Drawable.Winforms.Extensions
{
    public static class WinformsExtensions
    {
        public static Color ToWindowsColor(this ColorARGB color)
        {
            var clr = Color.FromArgb((int)Math.Round(color.Alpha * 255), color.Red, color.Green, color.Blue);
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

        public static System.Drawing.Font ToWindowsFont(this FontFamilyCore font, double size)
        {
            return new System.Drawing.Font(font.ToWindowsFont(), (float)size);
        }

        public static System.Drawing.FontFamily ToWindowsFont(this FontFamilyCore font)
        {
            return new FontFamily(font.Name);
        }
    }
}