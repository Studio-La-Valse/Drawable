using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.HTML
{
    /// <summary>
    /// An experimental canvas that stores an html/svg string as a render target for the <see cref="HTMLBitmapPainter"/> bitmap painter.
    /// </summary>
    public class HTMLCanvas
    {
        private string content = "";
        private readonly int width;
        private readonly int height;

        public int Height => height;
        public int Width => width;

        public ColorRGB Background { get; set; } = ColorRGB.White;

        public HTMLCanvas(int width, int height)
        {
            this.width = width;
            this.height = height;
        }


        public string SVGContent() => $@"<svg  width=""100%"" height=""100%"" viewBox=""0 0 {width} {height}"" fill=""{Background}"">{InnerHtml()}</svg>";
        public string InnerHtml() => content;
        public void Add(string element)
        {
            content += element;
        }
    }
}
