using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.ContentWrappers;

namespace StudioLaValse.Drawable.Extensions
{
    /// <summary>
    /// Extensions to Bitmap Painters.
    /// </summary>
    public static class BitmapPainterExtensions
    {
        /// <summary>
        /// Draw the content wrapper recursively.
        /// </summary>
        /// <param name="bitmapPainter"></param>
        /// <param name="contentWrapper"></param>
        public static BaseBitmapPainter DrawContentWrapper(this BaseBitmapPainter bitmapPainter, BaseContentWrapper contentWrapper)
        {
            foreach (var wrapper in contentWrapper.SelectBreadth(p => p.GetContentWrappers()))
            {
                foreach (var element in wrapper.GetDrawableElements())
                {
                    bitmapPainter.DrawElement(element);
                }
            }

            return bitmapPainter;
        }
    }
}