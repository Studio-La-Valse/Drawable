using StudioLaValse.Drawable.Interaction.Private;
using StudioLaValse.Drawable.Interaction.Selection;

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

    /// <summary>
    /// Add another input observer that executes its responses regardless of the result of the first.
    /// </summary>
    /// <param name="observser"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public static IInputObserver Always(this IInputObserver observser, IInputObserver next)
    {
        var chainedObserver = new AlwaysInputObserver(observser, next);
        return chainedObserver;
    }

    /// <summary>
    /// Notifies the selection for key presses and releases.
    /// Added using the <see cref="Then(IInputObserver, IInputObserver)"/> method which means the selected <paramref name="inputObserver"/> must allow the behavior to happen.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="inputObserver"></param>
    /// <param name="selection"></param>
    /// <returns></returns>
    public static IInputObserver NotifySelection<TKey>(this IInputObserver inputObserver, SelectionWithKeyResponse<TKey> selection) where TKey : IEquatable<TKey>
    {
        return inputObserver.Then(selection);
    }

    /// <summary>
    /// Add a selection border that allows a group selection.
    /// Added using the <see cref="Then(IInputObserver, IInputObserver)"/> method which means the selected <paramref name="observer"/> must allow the behavior to happen.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="observer"></param>
    /// <param name="sceneManager"></param>
    /// <param name="selectionBorder"></param>
    /// <returns></returns>
    public static IInputObserver AddSelectionBorder<TKey>(this IInputObserver observer, SceneManager<TKey> sceneManager, SelectionBorder selectionBorder) where TKey : IEquatable<TKey>
    {
        var behavior = new SelectionBorderBehavior<TKey>(sceneManager, selectionBorder);
        return observer.Then(behavior);
    }

    /// <summary>
    /// Handle mass transformations on drag.    
    /// Added using the <see cref="Then(IInputObserver, IInputObserver)"/> method which means the selected <paramref name="observer"/> must allow the behavior to happen.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="observer"></param>
    /// <param name="sceneManager"></param>
    /// <returns></returns>
    public static IInputObserver AddMassTransformations<TKey>(this IInputObserver observer, SceneManager<TKey> sceneManager) where TKey : IEquatable<TKey>
    {
        var behavior = new MassTransformBehavior<TKey>(sceneManager);
        return observer.Then(behavior);
    }

    /// <summary>
    /// Add the default behavior by propagating through the visual tree.
    /// Added using the <see cref="Then(IInputObserver, IInputObserver)"/> method which means the selected <paramref name="observer"/> must allow the default behavior to happen.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="observer"></param>
    /// <param name="sceneManager"></param>
    /// <returns></returns>
    public static IInputObserver AddDefaultBehavior<TKey>(this IInputObserver observer, SceneManager<TKey> sceneManager) where TKey : IEquatable<TKey>
    {
        var behavior = new DefaultBehavior<TKey>(sceneManager);
        return observer.Then(behavior);
    }

    /// <summary>
    /// Calls <see cref="INotifyEntityChanged{TKey}.RenderChanges"/> after handling behavior. 
    /// Chained using <see cref="Always(IInputObserver, IInputObserver)"/> method which means it is executed regardless of the <paramref name="observer"/>'s result.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="observer"></param>
    /// <param name="notifyEntityChanged"></param>
    /// <returns></returns>
    public static IInputObserver AddRerender<TKey>(this IInputObserver observer, INotifyEntityChanged<TKey> notifyEntityChanged) where TKey: IEquatable<TKey>
    {
        var behavior = new RerenderBehavior<TKey>(notifyEntityChanged);
        return observer.Always(behavior);
    }
}
