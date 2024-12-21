using Example.Model;
using Example.Scene;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Winforms.Controls;
using StudioLaValse.Drawable.Winforms.Painters;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using StudioLaValse.Drawable.Text;
using StudioLaValse.Drawable.Interaction;
using StudioLaValse.Drawable;


namespace Example.Winforms
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

            var notifyEntityChanged = SceneManager<ElementId>.CreateObservable();

            var selection = SelectionManager<PersistentElement>.CreateDefault(e => e.ElementId).OnChangedNotify(notifyEntityChanged, e => e.ElementId).InterceptKeys();
            var keyGenerator = new IncrementalKeyGenerator();

            var graph = new PointsModel(keyGenerator, notifyEntityChanged);
            var scene = new VisualPoints(graph, selection, notifyEntityChanged);

            var canvas = new ControlContainer();
            var textMeasurer = new TextMeasurer();
            var canvasPainter = new GraphicsPainter(canvas, textMeasurer);
            var sceneManager = new InteractiveSceneManager<ElementId>(scene, canvasPainter);

            canvas.Subscribe(sceneManager.Then(selection));
            sceneManager.Rerender();

            var content = new ContentWrapperControl(canvas);
            Application.Run(new Form1(canvas));

        }
    }
}