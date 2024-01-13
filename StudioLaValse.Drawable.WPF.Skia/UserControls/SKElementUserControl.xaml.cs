using SkiaSharp;
using SkiaSharp.Views.Desktop;
using StudioLaValse.Drawable.Skia.Models;
using StudioLaValse.Drawable.Text;
using StudioLaValse.Drawable.WPF.Visuals;

namespace StudioLaValse.Drawable.WPF.Skia.UserControls
{
    /// <summary>
    /// The default Skia implementation for WPF purposes.
    /// </summary>
    public partial class SKElementUserControl : BaseInteractiveVisual
    {
        public List<Action<SKCanvas>> Cache = new();

        public SKElementUserControl()
        {
            InitializeComponent();

            this.SKElement.IgnorePixelScaling = true;

            ExternalTextMeasure.TextMeasurer = new SkiaTextMeasurer();
        }


        public override void Refresh() => this.SKElement.InvalidateVisual();

        public void SKElement_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var graphics = e.Surface.Canvas;
            graphics.Clear();
            graphics.Scale((float)Zoom, (float)Zoom);
            graphics.Translate((float)TranslateX, (float)TranslateY);

            foreach (var action in Cache)
            {
                action(graphics);
            }
        }

        private void BaseInteractiveVisual_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Focus();
        }
    }
}
