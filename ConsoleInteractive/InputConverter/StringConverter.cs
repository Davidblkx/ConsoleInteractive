using System;

namespace ConsoleInteractive.InputConverter
{
    /// <summary>
    /// Convert between a string and other type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StringConverter<T> : IStringConverter<T>
    {
        public Func<T, string> ToStringProvider { get; }
        public Func<string, T> ToObjectProvider { get; }

        public Type Target => typeof(T);

        public StringConverter(Func<T, string> stringProvider, Func<string, T> objectProvider) {
            ToStringProvider = stringProvider;
            ToObjectProvider = objectProvider;
        }

        public T ToObject(string value) => ToObjectProvider(value);

        public string ToString(T value) => ToStringProvider(value);

        public string ToString(object value) => 
            ToStringProvider(FromObject(value));

        object IStringConverter.ToObject(string value) => 
            ToBaseObject(ToObjectProvider(value));

        private T FromObject(object value) {
            if (value is T e) return e;
            throw new Exception($"Can't convert value to {Target}");
        }

        private object ToBaseObject(T value) {
            if (value is object e) return e;
            throw new Exception($"Can't convert {Target} to object");
        }
    }

    /// <summary>
    /// Convert between a string and other type
    /// </summary>
    public class StringConverter : IStringConverter
    {
        public Func<object, string> ToStringProvider { get; }
        public Func<string, object> ToObjectProvider { get; }

        public Type Target { get; }

        public StringConverter(
            Type type,
            Func<object, string> stringProvider,
            Func<string, object> objectProvider
        ) {
            Target = type;
            ToStringProvider = stringProvider;
            ToObjectProvider = objectProvider;
        }

        public string ToString(object value) => 
            ToStringProvider(value);

        public object ToObject(string value) => 
            ToObjectProvider(value);
    }
}