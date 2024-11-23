using ReactiveUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System;

namespace StudioLaValse.Drawable.Example.Avalonia.ViewModels;


public class ViewModelBase : INotifyPropertyChanged
{
    private readonly Dictionary<string, object> _values = [];


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
            PropertyChangedEventArgs e = new(propertyName);
            handler(this, e);
        }
    }
    private string GetPropertyName(LambdaExpression expression)
    {
        return expression.Body is not MemberExpression memberExpression
            ? throw new InvalidOperationException()
            : memberExpression.Member.Name;
    }
}
