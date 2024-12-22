namespace StudioLaValse.Drawable
{
    /// <summary>
    /// An enum defining how the entity should be invalidated.
    /// </summary>
    public enum RenderMethod
    {
        /// <summary>
        /// Shallow invalidation: only the drawable elements at surface level will be redrawn.
        /// </summary>
        Shallow = 0,
        /// <summary>
        /// Deep invalidation: redraw all drawable elements from all grandchildren.
        /// </summary>
        Deep = 1,
        /// <summary>
        /// Recursive invalidation: all grandchildren of the visual parent will be regenerated.
        /// </summary>
        Recursive = 2
    }
}
