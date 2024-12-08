using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioLaValse.Drawable;

/// <summary>
/// A delegate to get a key from an entity.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <param name="entity"></param>
/// <returns></returns>
public delegate TKey GetKey<TEntity, TKey>(TEntity entity) where TKey : IEquatable<TKey>;
