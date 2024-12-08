using StudioLaValse.Drawable.Exceptions;

namespace StudioLaValse.Drawable
{
    /// <summary>
    /// An enum defining the possible handling methods when an entity is not found during invalidation.
    /// </summary>
    public enum NotFoundHandler
    {
        /// <summary>
        /// Simply skip this entity.
        /// </summary>
        Skip = 0,
        /// <summary>
        /// Rerender the entire scene.
        /// </summary>
        Rerender = 1,
        /// <summary>
        /// Raise an <see cref="EntityNotFoundInVisualTreeException"/>.
        /// </summary>
        Throw = 2
    }
}
