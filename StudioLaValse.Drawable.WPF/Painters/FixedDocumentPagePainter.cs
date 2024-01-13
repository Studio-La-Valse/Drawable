using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.WPF.Extensions;
using StudioLaValse.Geometry;
using System.Windows.Documents;
using System.Windows.Markup;

namespace StudioLaValse.Drawable.WPF.Painters
{
    /// <summary>
    /// A <see cref="FixedPage"/> painter, used to generate <see cref="FixedDocument"/>s, which can be rendered to PDF.
    /// </summary>
    public class FixedDocumentPagePainter : BaseLazyBitmapPainter<FixedPage>
    {
        private readonly FixedPage document;


        public FixedDocumentPagePainter(FixedPage page) : base(page)
        {
            document = page;
        }


        public override void DrawBackground(ColorARGB colorARGB)
        {
            document.Background = colorARGB.ToWindowsBrush();
        }

        protected override void DrawElement(FixedPage canvas, DrawableLine line)
        {
            var uiElement = line.ToUIElement();

            FixedPage.SetLeft(uiElement, 0);
            FixedPage.SetTop(uiElement, 0);

            ((IAddChild)canvas).AddChild(uiElement);
        }

        protected override void DrawElement(FixedPage canvas, DrawableRectangle rectangle)
        {
            var uiElement = rectangle.ToUIElement();

            FixedPage.SetLeft(uiElement, rectangle.TopLeftX);
            FixedPage.SetTop(uiElement, rectangle.TopLeftY);

            ((IAddChild)canvas).AddChild(uiElement);
        }



        protected override void DrawElement(FixedPage canvas, DrawableText text)
        {
            var uiElement = text.ToUIElement();

            FixedPage.SetLeft(uiElement, text.TopLeftX);
            FixedPage.SetTop(uiElement, text.TopLeftY);

            ((IAddChild)canvas).AddChild(uiElement);
        }

        protected override void DrawElement(FixedPage canvas, DrawableEllipse ellipse)
        {
            var uiElement = ellipse.ToUIElement();

            FixedPage.SetLeft(uiElement, ellipse.CenterX - ellipse.Width / 2);
            FixedPage.SetTop(uiElement, ellipse.CenterY - ellipse.Height / 2);

            ((IAddChild)canvas).AddChild(uiElement);
        }

        protected override void DrawElement(FixedPage canvas, DrawablePolyline polyline)
        {
            ((IAddChild)canvas).AddChild(polyline.ToUIElement());
        }

        protected override void DrawElement(FixedPage canvas, DrawablePolygon polygon)
        {
            ((IAddChild)canvas).AddChild(polygon.ToUIElement());
        }

        public override void InitDrawing()
        {

        }

        protected override void DrawElement(FixedPage canvas, DrawableBezierCurve bezier)
        {
            throw new NotImplementedException();
        }
    }
}
