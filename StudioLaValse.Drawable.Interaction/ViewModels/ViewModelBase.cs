using System.ComponentModel;
using System.Linq.Expressions;

namespace StudioLaValse.Drawable.Interaction.ViewModels;

/// <summary>
/// Simple implementation for a base viewmodel.
/// </summary>
public class ViewModelBase : INotifyPropertyChanged
{
    private readonly Dictionary<string, object> _values = [];

    /// <summary>
    /// Set the value and raise.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertySelector"></param>
    /// <param name="value"></param>
    protected void SetValue<T>(Expression<Func<T>> propertySelector, T value)
    {
        var propertyName = GetPropertyName(propertySelector);

        SetValue(propertyName, value);
    }
    /// <summary>
    /// Set the value and raise.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="value"></param>
    /// <exception cref="ArgumentException"></exception>
    protected void SetValue<T>(string propertyName, T value)
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            throw new ArgumentException("Invalid property name", propertyName);
        }

        _values[propertyName] = value!;

        NotifyPropertyChanged(propertyName);
    }

    /// <summary>
    /// Get the value for the property name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertySelector"></param>
    /// <returns></returns>
    protected T GetValue<T>(Expression<Func<T>> propertySelector)
    {
        var propertyName = GetPropertyName(propertySelector);

        return GetValue<T>(propertyName);
    }
    /// <summary>
    /// Get the value for the property name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
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


    /// <summary>
    /// Handles property changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
    /// <summary>
    /// Notifies a property has changed.
    /// </summary>
    /// <param name="propertyName"></param>
    protected void NotifyPropertyChanged(string propertyName)
    {
        var handler = PropertyChanged;

        if (handler != null)
        {
            PropertyChangedEventArgs e = new(propertyName);
            handler(this, e);
        }
    }

    /// <summary>
    /// Gets the property name.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static string GetPropertyName(LambdaExpression expression)
    {
        return expression.Body is not MemberExpression memberExpression
            ? throw new InvalidOperationException()
            : memberExpression.Member.Name;
    }
}
