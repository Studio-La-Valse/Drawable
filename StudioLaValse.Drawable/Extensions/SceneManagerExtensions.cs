using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Private;
using StudioLaValse.Geometry;
using System.Diagnostics.CodeAnalysis;

namespace StudioLaValse.Drawable.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="SceneManager{TEntity, TKey}"/> class.
    /// </summary>
    public static class SceneManagerExtensions
    {
        /// <summary>
        /// Set the background of the specified <see cref="SceneManager{TEntity, TKey}"/> and return the same instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity</typeparam>
        /// /// <typeparam name="TKey">The type of entity</typeparam>
        /// <param name="sceneManager"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static SceneManager<TEntity, TKey> WithBackground<TEntity, TKey>(this SceneManager<TEntity, TKey> sceneManager, ColorARGB color) where TEntity : class where TKey : IEquatable<TKey>
        {
            sceneManager.Background = color;
            return sceneManager;
        }

        /// <summary>
        /// Rerender the scene of the specified <see cref="SceneManager{TEntity, TKey}"/> to the specified <see cref="BaseBitmapPainter"/> and return the original scene manager.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// /// <typeparam name="TKey"></typeparam>
        /// <param name="sceneManager"></param>
        /// <param name="bitmapTarget"></param>
        /// <returns></returns>
        public static SceneManager<TEntity, TKey> WithRerender<TEntity, TKey>(this SceneManager<TEntity, TKey> sceneManager, BaseBitmapPainter bitmapTarget) where TEntity : class where TKey : IEquatable<TKey>
        {
            sceneManager.Rerender(bitmapTarget);
            return sceneManager;
        }

        /// <summary>
        /// Try to find the associated <see cref="BaseVisualParent{TEntity}"/> that is associated with the specified entity in the scene of the specified <see cref="SceneManager{TEntity, TKey}"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sceneManager"></param>
        /// <param name="entity"></param>
        /// <param name="visualTreeBranch"></param>
        /// <returns></returns>
        public static bool TryLocate<TEntity>(this IEnumerable<BaseVisualParent<TEntity>> sceneManager, TEntity entity, [NotNullWhen(true)] out BaseVisualParent<TEntity>? visualTreeBranch) where TEntity : class
        {
            visualTreeBranch = sceneManager.FirstOrDefault(c => c.AssociatedElement == entity);
            return visualTreeBranch is not null;
        }

        /// <summary>
        /// Tries to find the visual element that is associated with the specified entity. Throws an exception if it is not found.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sceneManager"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static BaseVisualParent<TEntity> LocateOrThrow<TEntity>(this IEnumerable<BaseVisualParent<TEntity>> sceneManager, TEntity entity) where TEntity : class
        {
            if (sceneManager.TryLocate(entity, out var branch))
            {
                return branch;
            }

            throw new Exception("Specified branch not found in children.");
        }

        /// <summary>
        /// Create a generic, general purpose <see cref="IObserver{T}"/> that can subscribe to any <see cref="IObservable{T}"/>. The implementation invalidates a TEntity on <see cref="IObserver{T}.OnNext(T)"/> and renders changes in the scene on <see cref="IObserver{T}.OnCompleted"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// /// <typeparam name="TKey"></typeparam>
        /// <param name="sceneManager"></param>
        /// <param name="baseBitmapPainter"></param>
        /// <returns></returns>
        public static IObserver<InvalidationRequest<TEntity>> CreateObserver<TEntity, TKey>(this SceneManager<TEntity, TKey> sceneManager, BaseBitmapPainter baseBitmapPainter) where TEntity : class where TKey : IEquatable<TKey>
        {
            return new EntityObserver<TEntity, TKey>(sceneManager, baseBitmapPainter);
        }

        /// <summary>
        /// Draw the content wrapper recursively.
        /// </summary>
        /// <param name="bitmapPainter"></param>
        /// <param name="contentWrapper"></param>
        public static void DrawContentWrapper(this BaseBitmapPainter bitmapPainter, BaseContentWrapper contentWrapper)
        {
            foreach (var wrapper in contentWrapper.SelectRecursive(p => p.GetContentWrappers()))
            {
                foreach (var element in wrapper.GetDrawableElements())
                {
                    bitmapPainter.DrawElement(element);
                }
            }
        }
    }
}