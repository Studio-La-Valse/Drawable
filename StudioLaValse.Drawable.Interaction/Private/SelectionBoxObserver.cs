using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class SelectionBoxObserver : IObserver<BoundingBox>
    {
        private readonly ISelectionBorder selectionBorder;
        private readonly IInteractiveCanvas canvasSource;

        public SelectionBoxObserver(ISelectionBorder selectionBorder, IInteractiveCanvas canvasSource)
        {
            this.selectionBorder = selectionBorder;
            this.canvasSource = canvasSource;
        }

        public void OnCompleted()
        {
            selectionBorder.SetVisibility(false);
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(BoundingBox value)
        {
            var trueBox = canvasSource.CanvasToHost(value);

            selectionBorder.SetVisibility(true);
            selectionBorder.Set(trueBox);
        }
    }
}
