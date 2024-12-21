namespace StudioLaValse.Drawable.Exceptions
{
    /// <summary>
    /// An exception that is thrown when attempting to add an elemen to the visual tree that is already in the visual tree.
    /// </summary>
    public class EntityAlreadyInVisualTreeException : Exception
    {
        /// <inheritdoc/>
        public EntityAlreadyInVisualTreeException() : base()
        {

        }

        /// <inheritdoc/>
        public EntityAlreadyInVisualTreeException(string message) : base(message)
        {

        }
    }
}


