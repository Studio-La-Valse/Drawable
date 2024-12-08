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
        /// <returns></returns>
        public static SceneManager<TEntity, TKey> WithRerender<TEntity, TKey>(this SceneManager<TEntity, TKey> sceneManager) where TEntity : class where TKey : IEquatable<TKey>
        {
            sceneManager.Rerender();
            return sceneManager;
        }

        /// <summary>
        /// Creates an observer for an invalidation request that dispatches the observations to the specified scene manager.
        /// This implementation invalidates a TEntity on <see cref="IObserver{T}.OnNext(T)"/> and renders changes in the scene on <see cref="IObserver{T}.OnCompleted"/>.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// /// <typeparam name="TKey"></typeparam>
        /// <param name="sceneManager"></param>
        /// <returns></returns>
        public static IObserver<InvalidationRequest<TEntity>> CreateObserver<TEntity, TKey>(this SceneManager<TEntity, TKey> sceneManager) where TEntity : class where TKey : IEquatable<TKey>
        {
            return new EntityObserver<TEntity, TKey>(sceneManager);
        }

        /// <summary>
        /// Draw the content wrapper recursively.
        /// </summary>
        /// <param name="bitmapPainter"></param>
        /// <param name="contentWrapper"></param>
        public static BaseBitmapPainter DrawContentWrapper(this BaseBitmapPainter bitmapPainter, BaseContentWrapper contentWrapper)
        {
            foreach (var wrapper in contentWrapper.SelectBreadth(p => p.GetContentWrappers()))
            {
                foreach (var element in wrapper.GetDrawableElements())
                {
                    bitmapPainter.DrawElement(element);
                }
            }

            return bitmapPainter;
        }
    }
}