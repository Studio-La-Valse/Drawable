namespace StudioLaValse.Drawable.Exceptions
{
    /// <summary>
    /// An exception that is thrown when attempting to invalide an entity whose key is not found in the visual tree.
    /// </summary>
    public class EntityNotFoundInVisualTreeException : Exception
    {
        /// <inheritdoc/>
        public EntityNotFoundInVisualTreeException() : base()
        {

        }

        /// <inheritdoc/>
        public EntityNotFoundInVisualTreeException(string message) : base(message)
        {

        }
    }
}

