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
    /// <typeparam name="TKey">The entity key type</typeparam>
    public class SceneManager<TEntity, TKey> where TEntity : class where TKey : IEquatable<TKey>
    {
        private readonly Queue<InvalidationRequest<TEntity>> renderQueue = new();
        private readonly VisualTreeCache<TEntity, TKey> cache;
        private readonly VisualTree<TEntity> visualTree;
        private readonly GetKey<TEntity, TKey> keyExtractor;

        /// <summary>
        /// The background color of the scene. This value is passed to the associated <see cref="BaseBitmapPainter"></see> in the <see cref="RenderChanges(BaseBitmapPainter)"/> method.
        /// </summary>
        public ColorARGB? Background { get; set; }

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
        /// <param name="keyExtractor">The key extractor for entities. Note that entities do not have to implement equals, but the keys they provide do.</param>
        public SceneManager(BaseVisualParent<TEntity> scene, GetKey<TEntity, TKey> keyExtractor)
        {
            visualTree = new VisualTree<TEntity>(scene);
            cache = new VisualTreeCache<TEntity, TKey>(keyExtractor);
            this.keyExtractor = keyExtractor;
        }


        /// <summary>
        /// Adds the specified entity to the invalidation queue.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>The same instance of the <see cref="SceneManager{TEntity, TKey}"/> to allow chaining of methods.</returns>
        public SceneManager<TEntity, TKey> AddToQueue(InvalidationRequest<TEntity> element)
        {
            if(renderQueue.Select(e => e.Entity).Any(e => keyExtractor(e).Equals(keyExtractor(element.Entity))))
            {
                return this;
            }
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
            if(Background is not null)
            {
                bitmapPainter.DrawBackground(Background);
            }

            while (renderQueue.Count != 0)
            {
                var entity = renderQueue.Dequeue();
                if(!cache.Find(entity.Entity, out var visualTree))
                {
                    switch (entity.NotFoundHandler)
                    {
                        case NotFoundHandler.Raise:
                            throw new InvalidOperationException(
                                $"Entity ({entity} : {keyExtractor(entity.Entity)}) was not found in the visual tree.");
                        case NotFoundHandler.Rerender:
                            Rerender(bitmapPainter);
                            return;
                        case NotFoundHandler.Skip:
                            continue;
                        default:
                            throw new NotImplementedException(nameof(entity.NotFoundHandler)); 
                    }
                }

                switch (entity.Method)
                {
                    case Method.Recursive:
                        visualTree.Regenerate();
                        break;
                    case Method.Deep:
                        visualTree.Rebuild();
                        break;
                    case Method.Shallow:
                        visualTree.Redraw();
                        break;
                    default:
                        throw new NotImplementedException(nameof(entity.Method));
                }
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
            renderQueue.Enqueue(new InvalidationRequest<TEntity>(visualTree.Element, NotFoundHandler.Raise));
            RenderChanges(bitmapTarget);
        }

        /// <summary>
        /// A static method to create a default observable that notifies when the state of an entity has changed. Used by any <see cref="IObserver{T}"/>, or <see cref="SceneManagerExtensions.CreateObserver{TEntity, TKey}(SceneManager{TEntity, TKey}, BaseBitmapPainter)"/> to invalidate and rerender these entities.
        /// </summary>
        /// <returns></returns>
        public static INotifyEntityChanged<TEntity> CreateObservable()
        {
            return new EntityInvalidator<TEntity>();
        }
    }

    /// <summary>
    /// A delegate to get a key from an entity.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public delegate TKey GetKey<TEntity, TKey>(TEntity entity) where TKey : IEquatable<TKey> where TEntity : class;
}
