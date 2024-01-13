﻿using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Drawable.Private;
using StudioLaValse.Geometry;

namespace StudioLaValse.Drawable
{
    /// <summary>
    /// The main object that invalidates Entity instances associated in the scene.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    public class SceneManager<TEntity> where TEntity : class, IEquatable<TEntity>
    {
        private readonly Queue<TEntity> renderQueue = new();
        private readonly VisualTreeCache<TEntity> cache = new();
        private readonly VisualTree<TEntity> visualTree;

        /// <summary>
        /// The background color of the scene. This value is passed to the associated <see cref="BaseBitmapPainter"></see> in the <see cref="RenderChanges(BaseBitmapPainter)"/> method.
        /// </summary>
        public ColorARGB Background { get; set; } = ColorARGB.White;

        /// <summary>
        /// Enumerates the visual parents in the scene.
        /// </summary>
        public IEnumerable<BaseVisualParent<TEntity>> VisualParents =>
            cache.Entries.Select(e => e.Item2.VisualParent);

        /// <summary>
        /// Enumerates the entities that are visually represented in the scene.
        /// </summary>
        public IEnumerable<TEntity> KnownEntities =>
            cache.Entries.Select(e => e.Item1);

        /// <summary>
        /// The default constructor
        /// </summary>
        /// <param name="scene"></param>
        public SceneManager(BaseVisualParent<TEntity> scene)
        {
            visualTree = new VisualTree<TEntity>(scene);
        }


        /// <summary>
        /// Adds the specified entity to the invalidation queue.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>The same instance of the <see cref="SceneManager{TEntity}"/> to allow chaining of methods.</returns>
        public SceneManager<TEntity> AddToQueue(TEntity element)
        {
            renderQueue.Enqueue(element);
            return this;
        }

        /// <summary>
        /// Render all changes in the invalidation queue to the specified <see cref="BaseBitmapPainter"/>.
        /// </summary>
        /// <param name="bitmapPainter"></param>
        public void RenderChanges(BaseBitmapPainter bitmapPainter)
        {
            cache.Rebuild(visualTree);

            bitmapPainter.InitDrawing();
            bitmapPainter.DrawBackground(Background);

            while (renderQueue.Count != 0)
            {
                var entity = renderQueue.Dequeue();
                var visualTree = cache.FindOrThrow(entity);
                visualTree.Unwrap();
            }

            visualTree.SelectRecursive(e => e.ChildBranches)
                .SelectMany(e => e.Elements)
                .ForEach(bitmapPainter.DrawElement);

            bitmapPainter.FinishDrawing();
        }

        /// <summary>
        /// Rerenders the original provided <see cref="BaseVisualParent{TEntity}"/> to a the specified <see cref="BaseBitmapPainter"/>.
        /// </summary>
        /// <param name="bitmapTarget"></param>
        public void Rerender(BaseBitmapPainter bitmapTarget)
        {
            renderQueue.Clear();
            renderQueue.Enqueue(visualTree.Element);
            RenderChanges(bitmapTarget);
        }

        /// <summary>
        /// A static method to create a default observable that notifies when the state of an entity has changed. Used by any <see cref="IObserver{T}"/>, or <see cref="SceneManagerExtensions.CreateObserver{TEntity}(SceneManager{TEntity}, BaseBitmapPainter)"/> to invalidate and rerender these entities.
        /// </summary>
        /// <returns></returns>
        public static INotifyEntityChanged<TEntity> CreateObservable()
        {
            return new EntityInvalidator<TEntity>();
        }
    }
}
