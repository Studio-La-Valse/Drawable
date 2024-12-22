namespace Example.Winforms
{
    public partial class Form1 : Form
    {
        public Form1(Control control)
        {
            InitializeComponent();

            this.Controls.Add(control);
            this.DoubleBuffered = true;
        }
    }
}