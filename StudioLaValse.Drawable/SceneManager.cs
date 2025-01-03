﻿using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Exceptions;
using StudioLaValse.Drawable.Extensions;
using StudioLaValse.Drawable.Private;
using StudioLaValse.Geometry;
using System.ComponentModel.DataAnnotations;

namespace StudioLaValse.Drawable
{

    /// <summary>
    /// The main object that invalidates Entity instances associated in the scene.
    /// </summary>
    /// <typeparam name="TKey">The entity key type</typeparam>
    public class SceneManager<TKey> where TKey : IEquatable<TKey>
    {
        private readonly Dictionary<TKey, InvalidationRequest<TKey>> renderQueue;
        private readonly VisualTreeCache<TKey> cache;
        private readonly VisualTree<TKey> visualTree;
        private readonly BaseBitmapPainter bitmapPainter;

        /// <summary>
        /// The background color of the scene. This value is passed to the associated <see cref="BaseBitmapPainter"></see> in the <see cref="RenderChanges()"/> method.
        /// </summary>
        public ColorARGB? Background { get; set; }

        /// <summary>
        /// The default constructor
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="bitmapPainter"></param>
        public SceneManager(BaseVisualParent<TKey> scene, BaseBitmapPainter bitmapPainter)
        {
            renderQueue = new Dictionary<TKey, InvalidationRequest<TKey>>(new EqualityComparer<TKey>());  
            visualTree = new VisualTree<TKey>(scene);
            cache = new VisualTreeCache<TKey>();
            this.bitmapPainter = bitmapPainter;
        }

        /// <summary>
        /// Adds the specified entity to the invalidation queue.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The same instance of the <see cref="SceneManager{TKey}"/> to allow chaining of methods.</returns>
        public SceneManager<TKey> AddToQueue(InvalidationRequest<TKey> request)
        {
            if (renderQueue.TryGetValue(request.Entity, out var existing))
            {
                var updatedExisting = new InvalidationRequest<TKey>(request.Entity,
                    request.NotFoundHandler.GetMax(existing.NotFoundHandler),
                    request.Method.GetMax(existing.Method));
                renderQueue[request.Entity] = updatedExisting;
            }
            else
            {
                renderQueue[request.Entity] = request;
            }
            return this;
        }

        /// <summary>
        /// Render all changes in the invalidation queue to the specified <see cref="BaseBitmapPainter"/>.
        /// </summary>
        public void RenderChanges()
        {
            bitmapPainter.InitDrawing();
            if(Background is not null)
            {
                bitmapPainter.DrawBackground(Background.Value);
            }

            if(renderQueue.Count == 0)
            {
                return;
            }

            cache.Rebuild(visualTree, renderQueue, out var missing);
            renderQueue.Clear();

            foreach (var entity in missing)
            {
                switch (entity.NotFoundHandler)
                {
                    case NotFoundHandler.Throw:
                        throw new EntityNotFoundInVisualTreeException(
                            $"Entity with key {entity} was not found in the visual tree.");
                    case NotFoundHandler.Rerender:
                        Rerender();
                        return;
                    case NotFoundHandler.Skip:
                        continue;
                    default:
                        throw new NotImplementedException(nameof(entity.NotFoundHandler));
                }
            }

            foreach(var entity in cache.Requests())
            {
                switch (entity.Value.Method)
                {
                    case RenderMethod.Recursive:
                        entity.Key.Regenerate();
                        break;
                    case RenderMethod.Deep:
                        entity.Key.Rebuild();
                        break;
                    case RenderMethod.Shallow:
                        entity.Key.Redraw();
                        break;
                    default:
                        throw new NotImplementedException(nameof(entity.Value.Method));
                }
            }

            visualTree.SelectBreadth(e => e.ChildBranches)
                .SelectMany(e => e.Elements)
                .ForEach(bitmapPainter.DrawElement);

            bitmapPainter.FinishDrawing();
        }

        /// <summary>
        /// Rerenders the original provided <see cref="BaseVisualParent{TEntity}"/> to a the specified <see cref="BaseBitmapPainter"/>.
        /// </summary>
        public void Rerender()
        {
            renderQueue.Clear();
            AddToQueue(new InvalidationRequest<TKey>(visualTree.Key, NotFoundHandler.Throw, RenderMethod.Recursive));
            RenderChanges();
        }

        /// <summary>
        /// Traverses the visual tree and handles behavior based on the provided function. 
        /// The handle behavior should return a boolean flag to indicate wether or not to continue the propagation into the visual tree.
        /// </summary>
        /// <param name="handleBehavior">The function to handle the behavior for each node in the tree.</param>
        public virtual bool TraverseAndHandle(Func<BaseVisualParent<TKey>, bool> handleBehavior)
        {
            return visualTree.TraverseAndHandle(handleBehavior);
        }

        /// <summary>
        /// A static method to create a default observable that notifies when the state of an entity has changed. 
        /// Used by any <see cref="IObserver{T}"/>, or <see cref="SceneManagerExtensions.CreateObserver{TKey}(SceneManager{TKey})"/> to invalidate and rerender these entities.
        /// </summary>
        /// <returns></returns>
        public static INotifyEntityChanged<TKey> CreateObservable()
        {
            return new EntityInvalidator<TKey>();
        }
    }
}
