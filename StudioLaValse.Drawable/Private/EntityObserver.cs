namespace StudioLaValse.Drawable.Private
{
    internal class EntityObserver<TEntity, TKey> : IObserver<InvalidationRequest<TEntity>> where TEntity : class where TKey: IEquatable<TKey>
    {
        private readonly SceneManager<TEntity, TKey> sceneManager;

        public EntityObserver(SceneManager<TEntity, TKey> sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public void OnCompleted()
        {
            sceneManager.RenderChanges();
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
