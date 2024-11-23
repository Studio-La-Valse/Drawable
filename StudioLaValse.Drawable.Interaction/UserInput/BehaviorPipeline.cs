using StudioLaValse.Drawable.Interaction.Private;

namespace StudioLaValse.Drawable.Interaction.UserInput
{
    /// <summary>
    /// An initial class for creating an interaction pipeline.
    /// </summary>
    public static class BehaviorPipeline
    {
        /// <summary>
        /// Creates an <see cref="IBehavior"/> that does nothing. This behavior, however, can be chained with other <see cref="IBehavior"/>'s to created the pipeline.
        /// </summary>
        /// <returns></returns>
        public static IBehavior DoNothing() => new EmptyPipeline();
    }
}
