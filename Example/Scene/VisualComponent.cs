namespace Example.Scene;

public class VisualComponent : BaseTransformableParent<ElementId>
{
    private readonly Component component;
    private readonly ISelectionManager<PersistentElement> selectionManager;
    private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;
    private bool isMouseOver;

    public XY Position => component.Position;
    public double Width { get; } = 100;
    public double Height { get; } = 50;
    public XY Center => Position + new XY(Width / 2, Height / 2);
    public XY Left => Center - new XY(Width / 2, 0);
    public XY Right => Center + new XY(Width / 2, 0);

    public VisualComponent(Component component, ISelectionManager<PersistentElement> selectionManager, INotifyEntityChanged<ElementId> notifyEntityChanged) : base(component.ElementId)
    {
        this.component = component;
        this.selectionManager = selectionManager;
        this.notifyEntityChanged = notifyEntityChanged;
    }

    public override bool IsSelected => selectionManager.IsSelected(component);

    protected override bool IsMouseOver
    {
        get => isMouseOver;
        set
        {
            if (value == isMouseOver)
            {
                return;
            }

            isMouseOver = value;
            notifyEntityChanged.Invalidate(component.ElementId, renderMethod: RenderMethod.Shallow);
        }
    }

    public Component Component => component;

    public override bool Select()
    {
        return selectionManager.Add(component);
    }

    public override bool Deselect()
    {
        return selectionManager.Remove(component);
    }

    public override void Transform(double deltaX, double deltaY)
    {
        var vector = new XY(deltaX, deltaY);
        component.SetPosition(component.Position + vector);
        notifyEntityChanged.Invalidate(component.ElementId, renderMethod: RenderMethod.Recursive);
        notifyEntityChanged.Invalidate(component.Incomming.Select(e => e.ElementId));
        notifyEntityChanged.Invalidate(component.Outgoing.Select(e => e.ElementId));
    }

    public override IEnumerable<BaseContentWrapper> GetContentWrappers()
    {
        yield break;
    }

    public override IEnumerable<BaseDrawableElement> GetDrawableElements()
    {
        var opacity = IsSelected ? 1 : isMouseOver ? 0.5 : 0;
        var fill = new ColorARGB(opacity, 255, 0, 0);
        yield return new DrawableRectangle(component.Position.X, component.Position.Y, 100, 50, fill, 2, ColorARGB.White);
        yield return new DrawableCircle(Left, 3, ColorARGB.White);
        yield return new DrawableCircle(Right, 3, ColorARGB.White);
    }

    public override bool CaptureMouse(XY point)
    {
        return BoundingBox().Contains(point);
    }

    public override BoundingBox BoundingBox()
    {
        return new BoundingBox(Position, Position + new XY(Width, Height));
    }
}
