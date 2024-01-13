namespace StudioLaValse.Drawable.Interaction.Private
{
    internal class CollectiveDisposable : IDisposable
    {
        private readonly Queue<IDisposable> disposables;

        public CollectiveDisposable(Queue<IDisposable> disposables)
        {
            this.disposables = disposables;
        }

        public void Dispose()
        {
            while (disposables.Count > 0)
            {
                var disposable = disposables.Dequeue();
                disposable.Dispose();
            }
        }
    }
}
