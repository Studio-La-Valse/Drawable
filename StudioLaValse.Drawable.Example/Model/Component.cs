using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Interaction.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System.ComponentModel;

namespace StudioLaValse.Drawable.Example.Model;

public class Graph : PersistentElement
{
    private readonly HashSet<Component> components = [];
    public IEnumerable<Component> Components => components;

    private readonly HashSet<Connection> connections = [];
    private readonly IKeyGenerator<int> keyGenerator;

    public IEnumerable<Connection> Connections => connections;

    public Graph(IKeyGenerator<int> keyGenerator) : base(keyGenerator)
    {
        this.keyGenerator = keyGenerator;

        var first = new Component(keyGenerator, new XY(100, 100));
        var second = new Component(keyGenerator, new XY(100, 300));
        var third = new Component(keyGenerator, new XY(300, 200));

        components.Add(first);
        components.Add(second);
        components.Add(third);

        Connect(first, third);
        Connect(second, third);
    }

    public void Connect(Component left, Component right)
    {
        var connection = new Connection(keyGenerator, left, right); 

        left.AddOutgoing(connection);
        right.AddIncoming(connection);

        connections.Add(connection);
    }

    public void Disconnect(Connection connection)
    {
        connection.Left.RemoveOutgoing(connection);
        connection.Right.RemoveIncoming(connection);

        connections.Remove(connection);
    }

    public void Remove(Component component)
    {
        foreach(var incoming in component.Incomming)
        {
            Disconnect(incoming);
        }

        foreach(var outgoing in component.Outgoing)
        {
            Disconnect(outgoing);
        }

        components.Remove(component);
    }
}

public class Component : PersistentElement
{
    private readonly HashSet<Connection> outgoing = [];
    private readonly HashSet<Connection> incomming = [];
    private XY position;


    public XY Position => position;
    public HashSet<Connection> Outgoing => this.outgoing;
    public HashSet<Connection> Incomming => this.incomming;


    public Component(IKeyGenerator<int> keyGenerator, XY position) : base(keyGenerator)
    {
        this.position = position;
    }

    public void SetPosition(XY value)
    {
        this.position = value;
    }

    public void AddIncoming(Connection connection)
    {
        incomming.Add(connection);
    }

    public void RemoveIncoming(Connection connection)
    {
        incomming.Remove(connection);
    }

    public void AddOutgoing(Connection connection)
    {
        outgoing.Add(connection);
    }

    public void RemoveOutgoing(Connection connection)
    {
        outgoing.Remove(connection);
    }
}

public class Connection : PersistentElement
{
    public Component Left { get; }
    public Component Right { get; }

    public Connection(IKeyGenerator<int> keyGenerator, Component left, Component right) : base(keyGenerator)
    {
        Left = left;
        Right = right;
    }
}

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
        if(ghostCurve is null)
        {
            yield break;
        }
        
        yield return ghostCurve;
    }

    public override bool HandleKeyUp(Interaction.UserInput.Key key)
    {
        foreach(var component in components)
        {
            if (component.IsSelected && key == Interaction.UserInput.Key.Delete)
            {
                graph.Remove(component.Component);
                notifyEntityChanged.Invalidate(graph.ElementId);
                return false;
            }
        }

        foreach (var connection in connections)
        {
            if (connection.IsSelected && key == Interaction.UserInput.Key.Delete)
            {
                graph.Disconnect(connection.Connection);
                notifyEntityChanged.Invalidate(graph.ElementId);
                return false;
            }
        }

        return base.HandleKeyUp(key);
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

        if(draggingFromLeft != null)
        {
            this.ghostCurve = VisualConnection.Curve(draggingFromLeft.Right, position, ColorARGB.White);
            notifyEntityChanged.Invalidate(graph.ElementId, method: Method.Shallow);
            return false;
        }

        if (draggingFromRight != null)
        {
            this.ghostCurve = VisualConnection.Curve(position, draggingFromRight.Left, ColorARGB.White);
            notifyEntityChanged.Invalidate(graph.ElementId, method: Method.Shallow);
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
            if(rightTarget != null)
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

public class VisualComponent : BaseTransformableParent<ElementId>
{
    private readonly Component component;
    private readonly ISelectionManager<PersistentElement> selectionManager;
    private readonly INotifyEntityChanged<ElementId> notifyEntityChanged;
    private bool isMouseOver;

    public XY Position => component.Position;
    public XY Center => Position + new XY(50, 25);
    public XY Left => Center - new XY(50, 0);
    public XY Right => Center + new XY(50, 0);

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
            notifyEntityChanged.Invalidate(component.ElementId, method: Method.Shallow);
        }
    }

    public Component Component => this.component;

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
        notifyEntityChanged.Invalidate(component.ElementId, method: Method.Recursive);
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
}

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
            notifyEntityChanged.Invalidate(connection.ElementId, method: Method.Shallow);
        }
    }
    public Connection Connection => this.connection;


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