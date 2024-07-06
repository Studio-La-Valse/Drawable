namespace StudioLaValse.Drawable.Avalonia.Controls;

internal class DefaultUnsubscriber<TEntity> : IDisposable
{
    private readonly IObserver<TEntity> observer;
    private readonly ISet<IObserver<TEntity>> observers;

    public DefaultUnsubscriber(IObserver<TEntity> observer, ISet<IObserver<TEntity>> observers)
    {
        this.observer = observer;
        this.observers = observers;
    }

    public void Dispose()
    {
        observers.Remove(observer);
    }
}
