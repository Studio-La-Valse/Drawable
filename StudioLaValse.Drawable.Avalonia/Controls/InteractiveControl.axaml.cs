using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using StudioLaValse.Drawable.Interaction.UserInput;
using System.Drawing;

namespace StudioLaValse.Drawable.Avalonia.Controls;
/// <inheritdoc/>
public partial class InteractiveControl : BaseInteractiveControl
{
    /// <inheritdoc/>
    public List<Action<DrawingContext>> DrawActions = [];

    /// <inheritdoc/>
    public InteractiveControl()
    {
        InitializeComponent();
    }

    /// <inheritdoc/>
    public override void Render(DrawingContext drawingContext)
    {
        drawingContext.PushTransform(new Matrix(Zoom, 0, 0, Zoom, 0,          0));
        drawingContext.PushTransform(new Matrix(1,    0, 0, 1,    TranslateX, TranslateY));

        foreach (var action in DrawActions)
        {
            action(drawingContext);
        }
    }

    /// <inheritdoc/>
    public override void Refresh()
    {
        InvalidateVisual();
    }
}
