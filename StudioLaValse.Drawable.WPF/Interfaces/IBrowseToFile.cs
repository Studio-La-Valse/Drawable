namespace StudioLaValse.Drawable.WPF.Interfaces
{
    public interface IBrowseToFile
    {
        string? OpenFileDialog(params string[] extensions);
        string? SaveFileDialog(params string[] extensions);
        string? BrowseDirectoryDialog();
    }
}
