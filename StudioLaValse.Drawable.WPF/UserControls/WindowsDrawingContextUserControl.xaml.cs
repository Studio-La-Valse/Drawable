using StudioLaValse.Drawable.WPF.Visuals;
using System.Windows.Media;

namespace StudioLaValse.Drawable.WPF.UserControls
{
    public partial class WindowsDrawingContextUserControl : BaseInteractiveVisual
    {
        public List<Action<DrawingContext>> Cache = new List<Action<DrawingContext>>();

        public WindowsDrawingContextUserControl()
        {
            InitializeComponent();
        }

        public override void Refresh()
        {
            using var drawingContext = this.drawingVisual.RenderOpen();
            drawingContext.PushTransform(new ScaleTransform(Zoom, Zoom));
            drawingContext.PushTransform(new TranslateTransform(TranslateX, TranslateY));

            foreach (var action in Cache)
            {
                action(drawingContext);
            }

            drawingContext.Close();
        }

        private void BaseInteractiveVisual_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            base.Focus();
        }
    }
}
