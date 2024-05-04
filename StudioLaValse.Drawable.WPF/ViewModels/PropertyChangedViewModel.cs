using System.ComponentModel;
using System.Linq.Expressions;


namespace StudioLaValse.Drawable.WPF.ViewModels
{
    /// <summary>
    /// An abstract class meant to be overriden by all viewmodels. Exposes the <see cref="GetValue{T}(Expression{Func{T}})"/> and <see cref="SetValue{T}(Expression{Func{T}}, T)"/> methods to simplify the property changed notifications.
    /// </summary>
    public abstract class PropertyChangedViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> _values = new();


        protected void SetValue<T>(Expression<Func<T>> propertySelector, T value)
        {
            var propertyName = GetPropertyName(propertySelector);

            SetValue(propertyName, value);
        }
        protected void SetValue<T>(string propertyName, T value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            _values[propertyName] = value!;

            NotifyPropertyChanged(propertyName);
        }


        protected T GetValue<T>(Expression<Func<T>> propertySelector)
        {
            var propertyName = GetPropertyName(propertySelector);

            return GetValue<T>(propertyName);
        }
        protected T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            if (!_values.TryGetValue(propertyName, out var value))
            {
                value = default(T);

                _values.Add(propertyName, value!);
            }

            return (T)value!;
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        private string GetPropertyName(LambdaExpression expression)
        {
            if (expression.Body is not MemberExpression memberExpression)
            {
                throw new InvalidOperationException();
            }

            return memberExpression.Member.Name;
        }
    }
}
