namespace StudioLaValse.Drawable.Text
{
    public static class ExternalTextMeasure
    {
        private static IMeasureText? measureText;

        /// <summary>
        /// Returns the registered <see cref="IMeasureText"/> instance. If none is registered, an exception is thrown. Register a platform specific implementation by setting the <see cref="TextMeasurer"/> property.
        /// </summary>
        public static IMeasureText TextMeasurer
        {
            get
            {
                return measureText ?? throw new Exception("Please register an external text measure tool first.");
            }
            set
            {
                measureText = value;
            }
        }
    }
}
