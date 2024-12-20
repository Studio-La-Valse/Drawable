namespace StudioLaValse.Drawable.Example.Model
{
    public static class DoubleExtensions
    {
        public static double Map(this double value, double minStart, double maxStart, double minEnd, double maxEnd)
        {
            var fraction = maxStart - minStart;

            if (fraction == 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            return minEnd + (maxEnd - minEnd) * ((value - minStart) / fraction);
        }

        public static double Clip(this double value, double min, double max)
        {
            value = Math.Min(value, max);
            value = Math.Max(value, min);
            return value;
        }
    }
}
