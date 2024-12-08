using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Example.Scene;
using StudioLaValse.Drawable.HTML;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System.Diagnostics;

namespace StudioLaValse.Drawable.Example.Svg;

internal class Program
{
    static void Main(string[] args)
    {
        // note how a static svg canvas does not need a text measurer!

        var keyGenerator = new IncrementalKeyGenerator();
        var model = new TextModel(keyGenerator);
        var scene = new TextScene(model);

        var canvas = new HTMLCanvas(500, 500);
        var canvasPainter = new HTMLCanvasPainter(canvas);
        var sceneManager = new SceneManager<ElementId>(scene, canvasPainter);

        sceneManager.Rerender();

        var file = Path.Combine(Environment.CurrentDirectory, "index.html");
        var svgContent = canvas.SVGContent();
        File.WriteAllText(file, svgContent);

        using var fileopener = new Process();

        fileopener.StartInfo.FileName = "explorer";
        fileopener.StartInfo.Arguments = "\"" + file + "\"";
        fileopener.Start();
    }
}
