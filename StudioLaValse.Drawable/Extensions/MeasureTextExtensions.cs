using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Text
{
    /// <summary>
    /// Extensions to the <see cref="IMeasureText"/> interface.
    /// </summary>
    public static class MeasureTextExtensions
    {
        /// <summary>
        /// Uses a LRU cache with the specified size.
        /// </summary>
        /// <param name="measureText"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IMeasureText UseCache(this IMeasureText measureText, int size)
        {
            var cache = new LRUCache<TextMeasurementKey, XY>(size);
            var measurer = new MeasureTextWithCache(measureText, cache);
            return measurer;
        }
    }
}
