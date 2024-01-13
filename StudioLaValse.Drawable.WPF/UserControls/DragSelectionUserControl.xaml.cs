using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using System.Windows;
using System.Windows.Controls;

namespace StudioLaValse.Drawable.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for DragSelectionUserControl.xaml
    /// </summary>
    public partial class DragSelectionUserControl : UserControl, ISelectionBorder
    {
        public DragSelectionUserControl()
        {
            InitializeComponent();
        }

        public void Set(BoundingBox boundingBox)
        {
            Canvas.SetLeft(dragSelectionBorder, boundingBox.MinPoint.X);
            Canvas.SetTop(dragSelectionBorder, boundingBox.MinPoint.Y);

            dragSelectionBorder.Width = boundingBox.Width;
            dragSelectionBorder.Height = boundingBox.Height;
        }

        public void SetVisibility(bool visible)
        {
            this.dragSelectionBorder.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
