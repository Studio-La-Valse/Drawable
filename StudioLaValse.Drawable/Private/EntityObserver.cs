namespace StudioLaValse.Drawable.Private
{
    internal class EntityObserver<TKey> : IObserver<InvalidationRequest<TKey>> where TKey: IEquatable<TKey>
    {
        private readonly SceneManager<TKey> sceneManager;

        public EntityObserver(SceneManager<TKey> sceneManager)
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

        public void OnNext(InvalidationRequest<TKey> value)
        {
            sceneManager.AddToQueue(value);
        }
    }
}
