using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Example.Scene;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Winforms.Controls;
using StudioLaValse.Drawable.Winforms.Painters;
using StudioLaValse.Geometry;
using StudioLaValse.Key;

namespace StudioLaValse.Drawable.Example.Winforms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var selection = SelectionManager<PersistentElement>.CreateDefault();
            var keyGenerator = new IncrementalKeyGenerator();
            var components = Enumerable.Range(0, 5000).Select(i => new ComponentModel(keyGenerator, new BaseGhost(keyGenerator))).ToArray();

            var graph = new GraphModel(keyGenerator, components);
            var scene = new VisualGraph(graph, selection);

            var canvas = new ControlContainer();
            var canvasPainter = new GraphicsPainter(canvas);
            var sceneManager = new SceneManager<PersistentElement>(scene)
                .WithBackground(ColorARGB.Black)
                .WithRerender(canvasPainter);

            //var userInput = sceneManager.GetUserInputDispatcher(canvas, canvas);
            //canvas.EnableZoom().EnablePan().EnableInteraction(userInput);

            var content = new ContentWrapperControl(canvas);
            Application.Run(new Form1(canvas));

        }
    }
}