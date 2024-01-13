using StudioLaValse.Drawable.Interaction.Private;

namespace StudioLaValse.Drawable.Interaction.UserInput
{
    /// <summary>
    /// An initial class for creating an interaction pipeline.
    /// </summary>
    public static class Pipeline
    {
        /// <summary>
        /// Creates an <see cref="IPipe"/> that does nothing. This pipe, however, can be chained with other <see cref="IPipe"/>'s to created the pipeline.
        /// </summary>
        /// <returns></returns>
        public static IPipe DoNothing() => new EmptyPipeline();
    }
}
