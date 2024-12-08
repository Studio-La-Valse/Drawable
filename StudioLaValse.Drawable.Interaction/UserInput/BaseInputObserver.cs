using StudioLaValse.Drawable.Interaction;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable.Interaction.UserInput;

/// <summary>
/// An input observer that does not actually do anything, but allows for chaining.
/// </summary>
public class BaseInputObserver : IInputObserver
{
    /// <inheritdoc/>
    public BaseInputObserver()
    {

    }
    /// <inheritdoc/>
    public virtual bool HandleLeftMouseButtonDown()
    {
        return true;
    }
    /// <inheritdoc/>
    public virtual bool HandleLeftMouseButtonUp()
    {
        return true;
    }
    /// <inheritdoc/>
    public virtual bool HandleMouseWheel(double delta)
    {
        return true;
    }
    /// <inheritdoc/>
    public virtual bool HandleRightMouseButtonDown()
    {
        return true;
    }
    /// <inheritdoc/>
    public virtual bool HandleRightMouseButtonUp()
    {
        return true;
    }
    /// <inheritdoc/>
    public virtual bool HandleSetMousePosition(XY position)
    {
        return true;
    }
    /// <inheritdoc/>
    public virtual bool HandleKeyDown(Key key)
    {
        return true;
    }
    /// <inheritdoc/>
    public virtual bool HandleKeyUp(Key key)
    {
        return true;
    }
}
