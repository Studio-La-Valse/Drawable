namespace StudioLaValse.Drawable.Private
{
    internal static class EnumExtensions
    {
        public static Method GetMax(this Method left, Method right)
        {
            return left > right ? left : right;
        }

        public static NotFoundHandler GetMax(this NotFoundHandler left, NotFoundHandler right)
        {
            return left > right ? left : right;
        }
    }
}
