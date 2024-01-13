using StudioLaValse.Drawable.Interaction.Private;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Extensions
{
    public static class SelctionBorderExtensions
    {
        /// <summary>
        /// Creates a default observer that can subscribe to a <see cref="IObserver{BoundingBox}"/> and sets the selection box according to the bounding box signal.
        /// </summary>
        /// <param name="selectionBorder"></param>
        /// <param name="canvasSource"></param>
        /// <returns></returns>
        public static IObserver<BoundingBox> CreateObserver(this ISelectionBorder selectionBorder, IInteractiveCanvas canvasSource)
        {
            return new SelectionBoxObserver(selectionBorder, canvasSource);
        }
    }
}
