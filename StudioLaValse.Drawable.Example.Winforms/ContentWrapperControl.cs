using StudioLaValse.Drawable.Winforms.Controls;

namespace StudioLaValse.Drawable.Example.Winforms
{
    public partial class ContentWrapperControl : Panel
    {
        public ContentWrapperControl(BaseInteractiveControl control)
        {
            InitializeComponent();

            this.Controls.Add(control);
            this.Dock = DockStyle.Fill;
        }
    }
}
