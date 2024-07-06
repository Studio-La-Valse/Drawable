namespace StudioLaValse.Drawable.Avalonia.Controls;

internal class ButtonObservable : IObservable<bool>
{
    private readonly ISet<IObserver<bool>> observers = new HashSet<IObserver<bool>>();
    
    public ButtonObservable()
    {
        
    }

    public void Next(bool value)
    {
        foreach(var  observer in this.observers)
        {
            observer.OnNext(value);
        }
    }

    public IDisposable Subscribe(IObserver<bool> observer)
    {
        observers.Add(observer);
        return new DefaultUnsubscriber<bool>(observer, observers);
    }
}
