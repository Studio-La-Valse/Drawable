namespace StudioLaValse.Drawable.Private
{
    internal static class EnumExtensions
    {
        public static RenderMethod GetMax(this RenderMethod left, RenderMethod right)
        {
            return left > right ? left : right;
        }

        public static NotFoundHandler GetMax(this NotFoundHandler left, NotFoundHandler right)
        {
            return left > right ? left : right;
        }
    }
}
