
# Getting Started


* Lets start with a basic scene and render it to html/svg:
```cs
using StudioLaValse.Drawable.HTML;
using StudioLaValse.Drawable.Extensions;

public class Program
{
    public static void Main(string[] args)
    {
        var graph = new VisualGraph();
        var canvas = new HTMLCanvas(width: 1920, height: 1080);
        var canvasPainter = new HTMLCanvasPainter(canvas);
        canvasPainter.DrawContentWrapper(graph).FinishDrawing();

        var result = canvas.SVGContent();
        Console.WriteLine(result);
    }
}

public class VisualGraph : BaseContentWrapper
{
    public override IEnumerable<BaseDrawableElement> GetDrawableElements()
    {
        var radius = 50;
        var firstPoint = new XY(Random.Shared.Next(300), Random.Shared.Next(300));
        var secondPoint = new XY(Random.Shared.Next(300), Random.Shared.Next(300));

        // Draw two white circles with a random position and a radius of 50.
        yield return new DrawableCircle(firstPoint, radius, ColorARGB.White);
        yield return new DrawableCircle(secondPoint, radius, ColorARGB.White);
    }
}
```
Remember that you can natively render to WPF, Avalonia, Skia, Winforms, HTML/SVG, or you can implement to your own canvas adapter. 
We'll use HTML/SVG strings for sake of simplicity.

* Great, but I want to base my scene on a model that can be serialized/persisted independent of it's visual representation.
```cs
using StudioLaValse.Drawable.HTML;
using StudioLaValse.Drawable.Extensions;

public class Program
{
    public static void Main(string[] args)
    {
        // Create dependencies.
        var keyGenerator = new IncrementalKeyGenerator();

        // Create the models.
        var firstNode = new NodeModel(keyGenerator);
        var secondNode = new NodeModel(keyGenerator);

        // Create the visual representation, the canvas and the canvas painter.
        var graph = new VisualGraph(firstNode, secondNode);
        var canvas = new HTMLCanvas(width: 1920, height: 1080);
        var canvasPainter = new HTMLCanvasPainter(canvas);
        canvasPainter.DrawContentWrapper(graph).FinishDrawing();

        var result = canvas.SVGContent();
        Console.WriteLine(result);
    }
}

public class NodeModel
{
    public XY Position { get; }
    public int Id { get; }

    public NodeModel(IKeyGenerator<int> keyGenerator)
    {
        Id = keyGenerator.Generate();
        Position = new XY(Random.Shared.Next(300), Random.Shared.Next(300));
    }
}

public class VisualNode : BaseVisualParent<int>
{
    private readonly NodeModel node;
    private readonly double radius = 50;

    public VisualNode(NodeModel node) : base(node.Id)
    {
        this.node = node;
    }

    public override IEnumerable<BaseDrawableElement> GetDrawableElements()
    {
        Debug.Write($"Visual node with id {node.Id} has regenerated it's drawable elements.");
        yield return new DrawableCircle(node.Position, radius, ColorARGB.White);
    }
}

public class VisualGraph : BaseContentWrapper
{
    private readonly NodeModel firstNode;
    private readonly NodeModel secondNode;

    public VisualGraph(NodeModel firstNode, NodeModel secondNode)
    {
        this.firstNode = firstNode;
        this.secondNode = secondNode;
    }

    public override IEnumerable<BaseContentWrapper> GetContentWrappers()
    {
        yield return new VisualNode(firstNode);
        yield return new VisualNode(secondNode);
    }
}
```

* Because now the visual representation of the node is coupled to the ID of the node, we can rerender only parts of the scene, for example lets change the position of only the first node. Notice how we don't rerender the second node.
We use an observable that emits the ID of the model and some additional information about the render method.
If a scene manager is subscribed to the observable, it will render the visual tree branch (and its children) using the given id.
```cs
using StudioLaValse.Drawable.HTML;
using StudioLaValse.Drawable.Extensions;

public class Program
{
    public static void Main(string[] args)
    {
        // Create dependencies.
        var keyGenerator = new IncrementalKeyGenerator();
        var notifyEntityChanged = SceneManager<int>.CreateObservable();

        // Create the models.
        var firstNode = new NodeModel(keyGenerator, notifyEntityChanged);
        var secondNode = new NodeModel(keyGenerator, notifyEntityChanged);

        // Create the canvas, canvas painter and scene.
        var sceneId = keyGenerator.Generate();
        var graph = new VisualGraph(firstNode, secondNode, sceneId);

        // Create the canvas and canvas painter.
        var canvas = new HTMLCanvas(width: 1920, height: 1080);
        var canvasPainter = new HTMLCanvasPainter(canvas);

        // Create a scene manager and subscribe to the notify entity changed, to invalidate models to the canvas.
        var sceneManager = new SceneManager<int>(graph, canvasPainter)
            .WithBackground(ColorARGB.Black)
            .WithRerender();
        using var subscription = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

        var result = canvas.SVGContent();
        Console.WriteLine("--");
        Console.WriteLine(result);
        Console.WriteLine("--");

        // Change your model, and the changes will be reflected on the canvas.
        firstNode.Position = new XY(100, 100);

        result = canvas.SVGContent();
        Console.WriteLine("--");
        Console.WriteLine(result);
        Console.WriteLine("--");
    }
}

public class NodeModel
{
    private readonly INotifyEntityChanged<int> notifyEntityChanged;
    private XY position;
    public XY Position
    {
        get => position;
        set
        {
            position = value;

            // Invalidate this node.
            notifyEntityChanged.Invalidate(Id);

            // Immediately render changes.
            notifyEntityChanged.RenderChanges();
        }
    }

    public int Id { get; }

    public NodeModel(IKeyGenerator<int> keyGenerator, INotifyEntityChanged<int> notifyEntityChanged)
    {
        this.notifyEntityChanged = notifyEntityChanged;

        Id = keyGenerator.Generate();
        Position = new XY(Random.Shared.Next(300), Random.Shared.Next(300));
    }
}

public class VisualNode : BaseVisualParent<int>
{
    private readonly NodeModel node;
    private readonly double radius = 50;

    public VisualNode(NodeModel node) : base(node.Id)
    {
        this.node = node;
    }

    public override IEnumerable<BaseDrawableElement> GetDrawableElements()
    {
        Console.WriteLine("--");
        Console.WriteLine($"Visual node with id {node.Id} has regenerated it's drawable elements.");
        Console.WriteLine("--");
        yield return new DrawableCircle(node.Position, radius, ColorARGB.White);
    }
}

public class VisualGraph : BaseVisualParent<int>
{
    private readonly NodeModel firstNode;
    private readonly NodeModel secondNode;

    public VisualGraph(NodeModel firstNode, NodeModel secondNode, int sceneId) : base(sceneId)
    {
        this.firstNode = firstNode;
        this.secondNode = secondNode;
    }

    public override IEnumerable<BaseContentWrapper> GetContentWrappers()
    {
        yield return new VisualNode(firstNode);
        yield return new VisualNode(secondNode);
    }
}
```

- Excersise:
I leave it up to the reader to connect the two nodes with a line, that moves with its parent nodes when they are moved. 
Bonus points if the connection or the nodes may be added/deleted and allow for mouse/keyboard input.
You can find the answer under Example/Scene/VisualGraph.