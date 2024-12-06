namespace StudioLaValse.Drawable.Interaction.Exceptions
{
    /// <summary>
    /// An exception that is thrown when an invalid selection logic has been provided.
    /// </summary>
    public class InvalidSelectionProviderException : Exception
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidSelectionProviderException() : base("Please either provide a selection through the constructor, or override with your own logic.")
        {

        }
    }
}
