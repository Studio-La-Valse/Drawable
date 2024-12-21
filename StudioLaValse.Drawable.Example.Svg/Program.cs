using Example.Model;
using Example.Scene;
using StudioLaValse.Drawable;
using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Drawable.HTML;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System.Diagnostics;

namespace Example.Svg;


public class Program
{
    public static void Main(string[] args)
    {
        // Create dependencies.
        var keyGenerator = new IncrementalKeyGenerator();
        var notifyEntityChanged = SceneManager<int>.CreateObservable();

        // Create the models.
        var firstNode = new Node(keyGenerator, notifyEntityChanged);
        var secondNode = new Node(keyGenerator, notifyEntityChanged);

        // Create the canvas, canvas painter and scene.
        var sceneId = keyGenerator.Generate();
        var graph = new MyGraph(firstNode, secondNode, sceneId);

        // Create the canvas and canvas painter.
        var canvas = new HTMLCanvas(width: 1920, height: 1080);
        var canvasPainter = new HTMLCanvasPainter(canvas);

        // Create a scene manager and subscribe to the notify entity changed, to invalidate models to the canvas.
        var sceneManager = new SceneManager<int>(graph, canvasPainter)
            .WithBackground(ColorARGB.Black)
            .WithRerender();
        using var subscription = notifyEntityChanged.Subscribe(sceneManager.CreateObserver());

        var result = canvas.SVGContent();
        Console.WriteLine(result);
        Console.WriteLine("--");
        Console.WriteLine("--");

        // Change your model, and the changes will be reflected on the canvas.
        firstNode.Position = new XY(100, 100);

        result = canvas.SVGContent();
        Console.WriteLine(result);
        Console.WriteLine("--");
        Console.WriteLine("--");
    }
}

public class Node
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

    public Node(IKeyGenerator<int> keyGenerator, INotifyEntityChanged<int> notifyEntityChanged)
    {
        this.notifyEntityChanged = notifyEntityChanged;

        Id = keyGenerator.Generate();
        Position = new XY(Random.Shared.Next(300), Random.Shared.Next(300));
    }
}

public class VisualNode : BaseVisualParent<int>
{
    private readonly Node node;
    private readonly double radius = 50;

    public VisualNode(Node node) : base(node.Id)
    {
        this.node = node;
    }

    public override IEnumerable<BaseDrawableElement> GetDrawableElements()
    {
        Console.WriteLine($"Visual node with id {node.Id} has regenerated it's drawable elements.");
        Console.WriteLine("--");
        Console.WriteLine("--");
        yield return new DrawableCircle(node.Position, radius, ColorARGB.White);
    }
}

public class VisualGraph : BaseVisualParent<int>
{
    private readonly Node firstNode;
    private readonly Node secondNode;

    public VisualGraph(Node firstNode, Node secondNode, int sceneId) : base(sceneId)
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
