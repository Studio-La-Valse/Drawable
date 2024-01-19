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

        /// <summary>
        /// The height of the canvas.
        /// </summary>
        public int Height => height;
        /// <summary>
        /// The Width of the canvas.
        /// </summary>
        public int Width => width;
        /// <summary>
        /// The background color of the canvas.
        /// </summary>
        public ColorRGB Background { get; set; } = ColorRGB.White;

        /// <summary>
        /// The primary constructor.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public HTMLCanvas(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Draws the inner html to svg tags.
        /// </summary>
        /// <returns></returns>
        public string SVGContent() => $@"<svg  width=""100%"" height=""100%"" viewBox=""0 0 {width} {height}"" fill=""{Background}"">{InnerHtml()}</svg>";
        /// <summary>
        /// Renders the inner html to an svg formatted string.
        /// </summary>
        /// <returns></returns>
        public string InnerHtml() => content;
        /// <summary>
        /// Add an svg element to the content.
        /// </summary>
        /// <param name="element"></param>
        public void Add(string element)
        {
            content += element;
        }
    }
}
