using StudioLaValse.Drawable.BitmapPainters;

namespace StudioLaValse.Drawable.Private
{
    internal class EntityObserver<TEntity> : IObserver<TEntity> where TEntity : class, IEquatable<TEntity>
    {
        private readonly SceneManager<TEntity> sceneManager;
        private readonly BaseBitmapPainter baseBitmapPainter;

        public EntityObserver(SceneManager<TEntity> sceneManager, BaseBitmapPainter baseBitmapPainter)
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

        public void OnNext(TEntity value)
        {
            sceneManager.AddToQueue(value);
        }
    }
}
