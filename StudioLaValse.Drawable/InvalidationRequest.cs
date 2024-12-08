namespace StudioLaValse.Drawable
{
    /// <summary>
    /// An invalidation request record.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="Entity"></param>
    /// <param name="NotFoundHandler"></param>
    /// <param name="Method"></param>
    public record InvalidationRequest<TEntity>(TEntity Entity, NotFoundHandler NotFoundHandler = NotFoundHandler.Throw, Method Method = Method.Recursive);
}
