using System;
namespace ConsoleInteractive.InputConverter
{
    /// <summary>
    /// Convert a string to and from a object
    /// </summary>
    public interface IStringConverter
    {
        Type Target { get; }

        object ToObject(string value);
        string ToString(object value);
    }

    public interface IStringConverter<T> : IStringConverter
    {
        new T ToObject(string value);
        string ToString(T value);
    }
}