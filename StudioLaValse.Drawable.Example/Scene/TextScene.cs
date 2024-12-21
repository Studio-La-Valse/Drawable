using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using Example.Model;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using StudioLaValse.Drawable.Text;

namespace Example.Scene;
public class TextScene : BaseVisualParent<ElementId>
{
    private readonly TextModel textModel;

    public TextScene(TextModel textModel) : base(textModel.ElementId)
    {
        this.textModel = textModel;
    }

    public override IEnumerable<BaseContentWrapper> GetContentWrappers()
    {
        yield break;
    }

    public override IEnumerable<BaseDrawableElement> GetDrawableElements()
    {
        var top = 20;
        var left = 20;

        var text = "Hello, world!";
        var textColor = new ColorARGB(255, 150, 100, 50);
        var circleColor = new ColorARGB(255, 255, 0, 0);
        var fontFamily = new FontFamilyCore("Arial");

        yield return new DrawableCircle(left, top, 2, circleColor);
        yield return new DrawableText(left, top, text, 12, textColor, HorizontalTextOrigin.Left, VerticalTextOrigin.Top, fontFamily);

        left += 100;
        yield return new DrawableCircle(left, top, 2, circleColor);
        yield return new DrawableText(left, top, text, 12, textColor, HorizontalTextOrigin.Left, VerticalTextOrigin.Center, fontFamily);

        left += 100;
        yield return new DrawableCircle(left, top, 2, circleColor);
        yield return new DrawableText(left, top, text, 12, textColor, HorizontalTextOrigin.Left, VerticalTextOrigin.Bottom, fontFamily);
        left = 20;

        top += 40;
        yield return new DrawableCircle(left, top, 2, circleColor);
        yield return new DrawableText(left, top, text, 12, textColor, HorizontalTextOrigin.Center, VerticalTextOrigin.Top, fontFamily);

        left += 100;
        yield return new DrawableCircle(left, top, 2, circleColor);
        yield return new DrawableText(left, top, text, 12, textColor, HorizontalTextOrigin.Center, VerticalTextOrigin.Center, fontFamily);

        left += 100;
        yield return new DrawableCircle(left, top, 2, circleColor);
        yield return new DrawableText(left, top, text, 12, textColor, HorizontalTextOrigin.Center, VerticalTextOrigin.Bottom, fontFamily);
        left = 20;

        top += 40;
        yield return new DrawableCircle(left, top, 2, circleColor);
        yield return new DrawableText(left, top, text, 12, textColor, HorizontalTextOrigin.Right, VerticalTextOrigin.Top, fontFamily);

        left += 100;
        yield return new DrawableCircle(left, top, 2, circleColor);
        yield return new DrawableText(left, top, text, 12, textColor, HorizontalTextOrigin.Right, VerticalTextOrigin.Center, fontFamily);

        left += 100;
        yield return new DrawableCircle(left, top, 2, circleColor);
        yield return new DrawableText(left, top, text, 12, textColor, HorizontalTextOrigin.Right, VerticalTextOrigin.Bottom, fontFamily);
        left = 20;
    }

    // Required to prevent text measuring without a text measurer.
    public override BoundingBox BoundingBox()
    {
        return new BoundingBox(-50, 310, 10, 110);
    }
}
