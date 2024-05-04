using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using StudioLaValse.Drawable.Interaction.UserInput;
using System.Drawing;

namespace StudioLaValse.Drawable.Avalonia.Controls;

public partial class InteractiveControl : BaseInteractiveControl
{
    public List<Action<DrawingContext>> DrawActions = [];

    public InteractiveControl()
    {
        InitializeComponent();
    }

    public override void Render(DrawingContext drawingContext)
    {
        drawingContext.PushTransform(new Matrix(Zoom, 0, 0, Zoom, 0,          0));
        drawingContext.PushTransform(new Matrix(1,    0, 0, 1,    TranslateX, TranslateY));

        foreach (var action in DrawActions)
        {
            action(drawingContext);
        }
    }


    public override void Refresh()
    {
        InvalidateVisual();
    }
}
