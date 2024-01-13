using System.Windows;

namespace StudioLaValse.Drawable.WPF.DependencyProperties
{
    public static class DependencyPropertyBase
    {
        /// <summary>
        /// Registers a <see cref="DependencyProperty"/> with default parameters.
        /// </summary>
        /// <typeparam name="TOwner"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="name"></param>
        /// <param name="changed"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="Exception">Thrown when incorrect types provided.</exception>
        public static DependencyProperty Register<TOwner, TProperty>(string name, Action<TOwner, TProperty> changed, TProperty? defaultValue = default)
        {
            return DependencyProperty.Register(name, typeof(TProperty), typeof(TOwner), new PropertyMetadata(defaultValue, (sender, args) =>
            {
                if (sender is not TOwner _sender)
                {
                    throw new Exception($"Dependency object is not of type {typeof(TOwner)}");
                }

                if (args.NewValue is not TProperty newValue)
                {
                    throw new Exception($"Target Dependency Object is not of type {typeof(TProperty)}");
                }

                changed(_sender, newValue);
            }));
        }
    }
}
