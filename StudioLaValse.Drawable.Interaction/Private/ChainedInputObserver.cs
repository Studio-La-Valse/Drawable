using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.Private;

internal class ChainedInputObserver : IInputObserver
{
    private readonly IInputObserver observer;
    private readonly IInputObserver next;

    public ChainedInputObserver(IInputObserver observer, IInputObserver next)
    {
        this.observer = observer;
        this.next = next;
    }

    public bool HandleKeyDown(Key key)
    {
        if (observer.HandleKeyDown(key))
        {
            return next.HandleKeyDown(key);
        }

        return false;
    }

    public bool HandleKeyUp(Key key)
    {
        if (observer.HandleKeyUp(key))
        {
            return next.HandleKeyUp(key);
        }

        return false;
    }

    public bool HandleLeftMouseButtonDown()
    {
        if (observer.HandleLeftMouseButtonDown())
        {
            return next.HandleLeftMouseButtonDown();
        }

        return false;
    }

    public bool HandleLeftMouseButtonUp()
    {
        if (observer.HandleLeftMouseButtonUp())
        {
            return next.HandleLeftMouseButtonUp();
        }

        return false;
    }

    public bool HandleMouseWheel(double delta)
    {
        if (observer.HandleMouseWheel(delta))
        {
            return next.HandleMouseWheel(delta);
        }

        return false;
    }

    public bool HandleRightMouseButtonDown()
    {
        if (observer.HandleRightMouseButtonDown())
        {
            return next.HandleRightMouseButtonDown();
        }

        return false;
    }

    public bool HandleRightMouseButtonUp()
    {
        if (observer.HandleRightMouseButtonUp())
        {
            return next.HandleRightMouseButtonUp();
        }

        return false;
    }

    public bool HandleMouseMove(XY position)
    {
        if (observer.HandleMouseMove(position))
        {
            return next.HandleMouseMove(position);
        }

        return false;
    }
}
