using Avalonia.Controls;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Avalonia.Controls;

/// <inheritdoc/>
public partial class SelectionBorder : UserControl, ISelectionBorder
{
    /// <inheritdoc/>
    public SelectionBorder()
    {
        InitializeComponent();
    }
    /// <inheritdoc/>
    public void Set(BoundingBox boundingBox)
    {
        Canvas.SetLeft(dragSelectionBorder, boundingBox.MinPoint.X);
        Canvas.SetTop(dragSelectionBorder, boundingBox.MinPoint.Y);

        dragSelectionBorder.Width = boundingBox.Width;
        dragSelectionBorder.Height = boundingBox.Height;
    }
    /// <inheritdoc/>
    public void SetVisibility(bool visible)
    {
        this.dragSelectionBorder.IsVisible = visible;
    }
}
