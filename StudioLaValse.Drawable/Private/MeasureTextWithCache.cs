using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Text
{
    internal class MeasureTextWithCache : IMeasureText
    {
        private readonly IMeasureText measureText;
        private readonly LRUCache<TextMeasurementKey, XY> cache;

        public MeasureTextWithCache(IMeasureText measureText, LRUCache<TextMeasurementKey, XY> cache)
        {
            this.measureText = measureText;
            this.cache = cache;
        }

        public XY Measure(string text, FontFamilyCore fontFamily, double size)
        {
            var key = new TextMeasurementKey(text, fontFamily, size);
            if(cache.TryGet(key, out var value))
            {
                return value;
            }

            value = measureText.Measure(text, fontFamily, size); 
            cache.Set(key, value);
            return value;
        }
    }
}
