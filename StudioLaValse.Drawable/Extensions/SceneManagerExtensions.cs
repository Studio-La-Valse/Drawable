using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Private;
using StudioLaValse.Geometry;
using System.Diagnostics.CodeAnalysis;

namespace StudioLaValse.Drawable.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="SceneManager{TEntity}"/> class.
    /// </summary>
    public static class SceneManagerExtensions
    {
        /// <summary>
        /// Set the background of the specified <see cref="SceneManager{TEntity}"/> and return the same instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity</typeparam>
        /// <param name="sceneManager"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static SceneManager<TEntity> WithBackground<TEntity>(this SceneManager<TEntity> sceneManager, ColorARGB color) where TEntity : class, IEquatable<TEntity>
        {
            sceneManager.Background = color;
            return sceneManager;
        }

        /// <summary>
        /// Rerender the scene of the specified <see cref="SceneManager{TEntity}"/> to the specified <see cref="BaseBitmapPainter"/> and return the original scene manager.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sceneManager"></param>
        /// <param name="bitmapTarget"></param>
        /// <returns></returns>
        public static SceneManager<TEntity> WithRerender<TEntity>(this SceneManager<TEntity> sceneManager, BaseBitmapPainter bitmapTarget) where TEntity : class, IEquatable<TEntity>
        {
            sceneManager.Rerender(bitmapTarget);
            return sceneManager;
        }

        /// <summary>
        /// Try to find the associated <see cref="BaseVisualParent{TEntity}"/> that is associated with the specified entity in the scene of the specified <see cref="SceneManager{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sceneManager"></param>
        /// <param name="entity"></param>
        /// <param name="visualTreeBranch"></param>
        /// <returns></returns>
        public static bool TryLocate<TEntity>(this IEnumerable<BaseVisualParent<TEntity>> sceneManager, TEntity entity, [NotNullWhen(true)] out BaseVisualParent<TEntity>? visualTreeBranch) where TEntity : class, IEquatable<TEntity>
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
        public static BaseVisualParent<TEntity> LocateOrThrow<TEntity>(this IEnumerable<BaseVisualParent<TEntity>> sceneManager, TEntity entity) where TEntity : class, IEquatable<TEntity>
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
        /// <param name="sceneManager"></param>
        /// <param name="baseBitmapPainter"></param>
        /// <returns></returns>
        public static IObserver<TEntity> CreateObserver<TEntity>(this SceneManager<TEntity> sceneManager, BaseBitmapPainter baseBitmapPainter) where TEntity : class, IEquatable<TEntity>
        {
            return new EntityObserver<TEntity>(sceneManager, baseBitmapPainter);
        }
    }
}
