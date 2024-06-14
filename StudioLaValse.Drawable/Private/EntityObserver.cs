using StudioLaValse.Drawable.BitmapPainters;

namespace StudioLaValse.Drawable.Private
{
    internal class EntityObserver<TEntity, TKey> : IObserver<InvalidationRequest<TEntity>> where TEntity : class where TKey: IEquatable<TKey>
    {
        private readonly SceneManager<TEntity, TKey> sceneManager;
        private readonly BaseBitmapPainter baseBitmapPainter;

        public EntityObserver(SceneManager<TEntity, TKey> sceneManager, BaseBitmapPainter baseBitmapPainter)
        {
            this.sceneManager = sceneManager;
            this.baseBitmapPainter = baseBitmapPainter;
        }

        public void OnCompleted()
        {
            sceneManager.RenderChanges(baseBitmapPainter);
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(InvalidationRequest<TEntity> value)
        {
            sceneManager.AddToQueue(value);
        }
    }
}
