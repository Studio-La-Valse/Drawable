using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioLaValse.Drawable.Interaction.Extensions;

/// <summary>
/// Extensions to the <see cref="IInputObserver"/> interface.
/// </summary>
public static class InputObserverExtensions
{
    /// <summary>
    /// Chains the first input observer with the next.
    /// The next is only executed if the respective method of the first returns true.
    /// If the second observer is executed, its result is returned.
    /// </summary>
    /// <param name="observer"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public static IInputObserver Then(this IInputObserver observer, IInputObserver next)
    {
        var chainedObserver = new ChainedInputObserver(observer, next);
        return chainedObserver;
    }
}

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

    public bool HandleSetMousePosition(XY position)
    {
        if (observer.HandleSetMousePosition(position))
        {
            return next.HandleSetMousePosition(position);
        }

        return false;
    }
}
