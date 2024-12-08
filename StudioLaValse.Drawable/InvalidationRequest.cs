namespace StudioLaValse.Drawable
{
    /// <summary>
    /// An invalidation request record.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="Entity"></param>
    /// <param name="NotFoundHandler"></param>
    /// <param name="Method"></param>
    public record InvalidationRequest<TKey>(TKey Entity, NotFoundHandler NotFoundHandler = NotFoundHandler.Throw, Method Method = Method.Recursive) where TKey : IEquatable<TKey>;
}
