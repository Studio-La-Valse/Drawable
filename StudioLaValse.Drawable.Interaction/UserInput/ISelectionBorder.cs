using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.UserInput
{

    /// <summary>
    /// The base interface for a dynamic selection border.
    /// </summary>
    public interface ISelectionBorder
    {
        /// <summary>
        /// Called to toggle the selection border visibility.
        /// </summary>
        /// <param name="visible"></param>
        void SetVisibility(bool visible);

        /// <summary>
        /// Sets the selection border by the specified <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="boundingBox"></param>
        void Set(BoundingBox boundingBox);

        /// <summary>
        /// Creates a default observable bounding box which can be used to update the selection boder.
        /// </summary>
        /// <returns></returns>
        public static ObservableBoundingBox CreateObservable() => new ObservableBoundingBox();
    }
}