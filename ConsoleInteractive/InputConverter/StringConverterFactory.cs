using System;

namespace ConsoleInteractive.InputConverter
{
    /// <summary>
    /// Create a converter from [TYPE]
    /// </summary>
    public static class StringConverterFactory
    {
        /// <summary>
        /// Create IStringConverter for type [T]
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IStringConverter<T> Create<T>(StringConverterProvider? provider = default) {
            if ((provider ?? StringConverterProvider.Global)
                .TryGetConverter<T>(out var c1)) return c1;

            var instance = Activator.CreateInstance<T>();

            if (instance is IStringConverter<T> c2) return c2;
            if (instance is IConvertible)
                return new StringConverter<T>(
                    e => Convert.ToString(e),
                    e => (T)Convert.ChangeType(e, typeof(T))
                );

            throw new Exception("Can't create converter from type");
        }

        /// <summary>
        /// Create IStringConverter for type [T]
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IStringConverter Create(Type type, StringConverterProvider? provider = default) {
            if ((provider ?? StringConverterProvider.Global)
                .TryGetConverter(type, out var c1)) return c1;

            var instance = Activator.CreateInstance(type);

            if (instance is IStringConverter c2) return c2;
            if (instance is IConvertible)
                return new StringConverter(
                    type,
                    e => Convert.ToString(e),
                    e => Convert.ChangeType(e, type)
                );

            throw new Exception("Can't create converter from type");
        }

        /// <summary>
        /// Creates a converter that always throws
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IStringConverter<T> Empty<T>() {
            var type = typeof(T);
            return new StringConverter<T>(
                e => throw ConvertException.From<T>(),
                e => throw ConvertException.From<T>()
            );
        }

        /// <summary>
        /// Creates a converter that always throws
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IStringConverter Empty(Type type) {
            return new StringConverter(
                type,
                e => throw ConvertException.From(type),
                e => throw ConvertException.From(type)
            );
        }
    }
}