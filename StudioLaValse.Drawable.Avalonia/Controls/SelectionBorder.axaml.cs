using Avalonia.Controls;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Avalonia.Controls;
public partial class SelectionBorder : UserControl, ISelectionBorder
{
    public SelectionBorder()
    {
        InitializeComponent();
    }

    public void Set(BoundingBox boundingBox)
    {
        Canvas.SetLeft(dragSelectionBorder, boundingBox.MinPoint.X);
        Canvas.SetTop(dragSelectionBorder, boundingBox.MinPoint.Y);

        dragSelectionBorder.Width = boundingBox.Width;
        dragSelectionBorder.Height = boundingBox.Height;
    }

    public void SetVisibility(bool visible)
    {
        this.dragSelectionBorder.IsVisible = visible;
    }
}
