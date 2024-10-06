using StudioLaValse.Drawable.Example.Model;
using StudioLaValse.Drawable.Example.Scene;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Winforms.Controls;
using StudioLaValse.Drawable.Winforms.Painters;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using StudioLaValse.Drawable.Text;


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

            var notifyEntityChanged = SceneManager<PersistentElement, ElementId>.CreateObservable();

            var selection = SelectionManager<PersistentElement>.CreateDefault(e => e.ElementId);
            var keyGenerator = new IncrementalKeyGenerator();
            var components = Enumerable.Range(0, 5000).Select(i => new ComponentModel(keyGenerator, new BaseGhost(keyGenerator))).ToArray();

            var graph = new GraphModel(keyGenerator, components);
            var scene = new VisualGraph(graph, selection);

            var canvas = new ControlContainer();
            var textMeasurer = new TextMeasurer();
            ExternalTextMeasure.TextMeasurer = textMeasurer;
            var canvasPainter = new GraphicsPainter(canvas, textMeasurer);
            var sceneManager = new SceneManager<PersistentElement, ElementId>(scene, e => e.ElementId)
                .WithBackground(ColorARGB.Black)
                .WithRerender(canvasPainter);

            var pipe = Pipeline.DoNothing()
                .InterceptKeys(selection, out var _selectionManager)
                .ThenHandleDefaultMouseInteraction(sceneManager.VisualParents, notifyEntityChanged)
                .ThenHandleMouseHover(sceneManager.VisualParents, notifyEntityChanged)
                .ThenHandleDefaultClick(sceneManager.VisualParents, _selectionManager)
                .ThenHandleTransformations(_selectionManager, sceneManager.VisualParents, notifyEntityChanged)
                .ThenRender(notifyEntityChanged);

            var content = new ContentWrapperControl(canvas);
            Application.Run(new Form1(canvas));

        }
    }
}