using System.Linq;
using System;

namespace ConsoleInteractive.InputConverter
{
    /// <summary>
    /// Create a converter from [TYPE]
    /// </summary>
    public static class StringConverterFactory
    {
        private static readonly string THROW_MESSAGE = "Can't create converter from type";

        /// <summary>
        /// Create IStringConverter for type [T]
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IStringConverter<T> Create<T>(StringConverterProvider? provider = default) {
            if ((provider ?? StringConverterProvider.Global)
                .TryGetConverter<T>(out var c1)) return c1;
                
            if (Implements(typeof(T), typeof(IStringConverter<T>))) {
                var instance = Activator.CreateInstance<T>();
                if (instance is IStringConverter<T> c2) return c2;
            } else if (Implements(typeof(T), typeof(IConvertible))) {
                return new StringConverter<T>(
                    e => Convert.ToString(e),
                    e => (T)Convert.ChangeType(e, typeof(T))
                );
            }

            throw new Exception(THROW_MESSAGE);
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

            if (Implements(type, typeof(IStringConverter))) {
                var instance = Activator.CreateInstance(type);
                if (instance is IStringConverter c2) return c2;
            } else if (Implements(type, typeof(IConvertible))) {
                return new StringConverter(
                    type,
                    e => Convert.ToString(e),
                    e => Convert.ChangeType(e, type)
                );
            }

            throw new Exception(THROW_MESSAGE);
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

        /// <summary>
        /// Checks if source implements or extends target
        /// </summary>
        /// <param name="source"></param>
        /// <param name="Target"></param>
        /// <returns></returns>
        private static bool Implements(Type source, Type target) {
            return source.GetInterfaces().Any(x => x.IsGenericType ? 
                x.GetGenericTypeDefinition() == target : x == target);
        }
    }
}