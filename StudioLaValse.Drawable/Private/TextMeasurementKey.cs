namespace StudioLaValse.Drawable.Text
{
    internal class TextMeasurementKey : IEquatable<TextMeasurementKey>
    {
        public string Text { get; }
        public FontFamilyCore FontFamily { get; }
        public double FontSize { get; }

        public TextMeasurementKey(string text, FontFamilyCore fontFamily, double fontSize)
        {
            Text = text;
            FontFamily = fontFamily;
            FontSize = fontSize;
        }

        public bool Equals(TextMeasurementKey? other)
        {
            if (other is null)
            {
                return false;
            }

            return Text == other.Text && FontFamily.Equals(other.FontFamily) && FontSize == other.FontSize;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TextMeasurementKey);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Text, FontFamily, FontSize);
        }
    }
}
