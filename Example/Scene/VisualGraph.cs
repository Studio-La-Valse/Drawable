namespace Example.Scene;

public class VisualGraph : BaseInteractiveParent<ElementId>
{
    private readonly Graph graph;
    private readonly ISelectionManager<PersistentElement> selectionManager;
    private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;
    private readonly HashSet<VisualComponent> components = [];
    private readonly HashSet<VisualConnection> connections = [];

    public VisualGraph(Graph graph, ISelectionManager<PersistentElement> selectionManager, INotifyEntityChanged<ElementId> notifyEntityChanged) : base(graph.ElementId)
    {
        this.graph = graph;
        this.selectionManager = selectionManager;
        this.notifyEntityChanged = notifyEntityChanged;
    }

    public override IEnumerable<BaseContentWrapper> GetContentWrappers()
    {
        components.Clear();
        connections.Clear();

        foreach (var component in graph.Components)
        {
            var visualComponent = new VisualComponent(component, selectionManager, notifyEntityChanged);
            components.Add(visualComponent);
            yield return visualComponent;
        }

        foreach (var connection in graph.Connections)
        {
            var visualConnection = new VisualConnection(
                connection,
                components.Single(e => e.Key == connection.Left.ElementId),
                components.Single(e => e.Key == connection.Right.ElementId),
                selectionManager,
                notifyEntityChanged
            );
            connections.Add(visualConnection);
            yield return visualConnection;
        }
    }

    public override IEnumerable<BaseDrawableElement> GetDrawableElements()
    {
        if (ghostCurve is null)
        {
            yield break;
        }

        yield return ghostCurve;
    }

    public override bool HandleKeyUp(Key key)
    {
        var allowContinue = base.HandleKeyUp(key);
        foreach (var component in components)
        {
            if (component.IsSelected && key == StudioLaValse.Drawable.Interaction.UserInput.Key.Delete)
            {
                graph.Remove(component.Component);
                notifyEntityChanged.Invalidate(graph.ElementId);
                allowContinue = false;
            }
        }

        foreach (var connection in connections)
        {
            if (connection.IsSelected && key == StudioLaValse.Drawable.Interaction.UserInput.Key.Delete)
            {
                graph.Disconnect(connection.Connection);
                notifyEntityChanged.Invalidate(graph.ElementId);
                allowContinue = false;
            }
        }

        return allowContinue;
    }


    private VisualComponent? draggingFromLeft = null;
    private VisualComponent? draggingFromRight = null;
    public override bool HandleLeftMouseButtonDown()
    {
        if (!base.HandleLeftMouseButtonDown())
        {
            return false;
        }

        var draggingFromLeft = components.FirstOrDefault(e => e.Right.DistanceTo(LastMousePosition) < 2);
        if (draggingFromLeft != null)
        {
            this.draggingFromLeft = draggingFromLeft;
            return false;
        }

        var draggingFromRight = components.FirstOrDefault(e => e.Left.DistanceTo(LastMousePosition) < 2);
        if (draggingFromRight != null)
        {
            this.draggingFromRight = draggingFromRight;
            return false;
        }

        return true;
    }

    private DrawableBezierCubic? ghostCurve = null;
    public override bool HandleMouseMove(XY position)
    {
        if (!base.HandleMouseMove(position))
        {
            return false;
        }

        if (draggingFromLeft != null)
        {
            ghostCurve = VisualConnection.Curve(draggingFromLeft.Right, position, ColorARGB.White);
            notifyEntityChanged.Invalidate(graph.ElementId, renderMethod: RenderMethod.Shallow);
            return false;
        }

        if (draggingFromRight != null)
        {
            ghostCurve = VisualConnection.Curve(position, draggingFromRight.Left, ColorARGB.White);
            notifyEntityChanged.Invalidate(graph.ElementId, renderMethod: RenderMethod.Shallow);
            return false;
        }

        return true;
    }

    public override bool HandleLeftMouseButtonUp()
    {
        if (!base.HandleLeftMouseButtonUp())
        {
            return false;
        }

        var preventContinue = false;

        if (draggingFromLeft != null)
        {
            var rightTarget = components.FirstOrDefault(e => e.Left.DistanceTo(LastMousePosition) < 2);
            if (rightTarget != null)
            {
                graph.Connect(draggingFromLeft.Component, rightTarget.Component);
                preventContinue = true;
            }
        }

        if (draggingFromRight != null)
        {
            var leftTarget = components.FirstOrDefault(e => e.Right.DistanceTo(LastMousePosition) < 2);
            if (leftTarget != null)
            {
                graph.Connect(leftTarget.Component, draggingFromRight.Component);
                preventContinue = true;
            }
        }

        draggingFromLeft = null;
        draggingFromRight = null;
        ghostCurve = null;
        notifyEntityChanged.Invalidate(graph.ElementId);

        if (preventContinue)
        {
            return false;
        }

        return true;
    }
}
