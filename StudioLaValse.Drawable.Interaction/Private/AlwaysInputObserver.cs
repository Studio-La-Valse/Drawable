using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private;

internal class AlwaysInputObserver : IInputObserver
{
    private readonly IInputObserver observer;
    private readonly IInputObserver next;

    public AlwaysInputObserver(IInputObserver observer, IInputObserver next)
    {
        this.observer = observer;
        this.next = next;
    }

    public bool HandleKeyDown(Key key)
    {
        if (observer.HandleKeyDown(key))
        {

        }

        return next.HandleKeyDown(key);
    }

    public bool HandleKeyUp(Key key)
    {
        if (observer.HandleKeyUp(key))
        {

        }

        return next.HandleKeyUp(key);
    }

    public bool HandleLeftMouseButtonDown()
    {
        if (observer.HandleLeftMouseButtonDown())
        {

        }

        return next.HandleLeftMouseButtonDown();
    }

    public bool HandleLeftMouseButtonUp()
    {
        if (observer.HandleLeftMouseButtonUp())
        {

        }

        return next.HandleLeftMouseButtonUp();
    }

    public bool HandleMouseWheel(double delta)
    {
        if (observer.HandleMouseWheel(delta))
        {

        }

        return next.HandleMouseWheel(delta);
    }

    public bool HandleRightMouseButtonDown()
    {
        if (observer.HandleRightMouseButtonDown())
        {

        }

        return next.HandleRightMouseButtonDown();
    }

    public bool HandleRightMouseButtonUp()
    {
        if (observer.HandleRightMouseButtonUp())
        {

        }

        return next.HandleRightMouseButtonUp();
    }

    public bool HandleMouseMove(XY position)
    {
        if (observer.HandleMouseMove(position))
        {

        }

        return next.HandleMouseMove(position);
    }
}
