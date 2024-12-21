namespace Example.Scene;

public class VisualConnection : BaseSelectableParent<ElementId>
{
    private readonly Connection connection;
    private readonly VisualComponent left;
    private readonly VisualComponent right;
    private readonly ISelectionManager<PersistentElement> selectionManager;
    private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;
    private bool isMouseOver;

    public override bool IsSelected => selectionManager.IsSelected(connection);
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
            notifyEntityChanged.Invalidate(connection.ElementId, renderMethod: RenderMethod.Shallow);
        }
    }
    public Connection Connection => connection;


    public DrawableBezierCubic Curve()
    {
        var color = IsSelected ? new ColorARGB(255, 0, 0) : isMouseOver ? new ColorARGB(0.5, 255, 0, 0) : ColorARGB.White;
        var firstPoint = left.Right;
        var fourthPoint = right.Left;

        return Curve(firstPoint, fourthPoint, color);
    }
    public static DrawableBezierCubic Curve(XY first, XY second, ColorARGB color)
    {
        var deltaY = first.Y - second.Y;
        var firstPoint = first;
        var secondPoint = firstPoint + new XY(Math.Abs(deltaY), 0);
        var fourthPoint = second;
        var thirdPoint = fourthPoint - new XY(Math.Abs(deltaY), 0);

        return new DrawableBezierCubic(firstPoint, secondPoint, thirdPoint, fourthPoint, color, 2);
    }

    public VisualConnection(Connection connection, VisualComponent left, VisualComponent right, ISelectionManager<PersistentElement> selectionManager, INotifyEntityChanged<ElementId> notifyEntityChanged) : base(connection.ElementId)
    {
        this.connection = connection;
        this.left = left;
        this.right = right;
        this.selectionManager = selectionManager;
        this.notifyEntityChanged = notifyEntityChanged;
    }


    public override bool Select()
    {
        return selectionManager.Add(connection);
    }
    public override bool Deselect()
    {
        return selectionManager.Remove(connection);
    }

    public override IEnumerable<BaseContentWrapper> GetContentWrappers()
    {
        yield break;
    }

    public override IEnumerable<BaseDrawableElement> GetDrawableElements()
    {
        yield return Curve();
    }

    public override bool CaptureMouse(XY point)
    {
        return Curve().ClosestPointEdge(point).DistanceTo(point) < 2;
    }
}