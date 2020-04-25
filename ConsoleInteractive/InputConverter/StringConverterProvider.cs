using System;
using System.Collections.Generic;
namespace ConsoleInteractive.InputConverter
{
    public class StringConverterProvider
    {
        private readonly Dictionary<Type, IStringConverter> _items =
            new Dictionary<Type, IStringConverter>();

        /// <summary>
        /// Register a new converter for a type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="converter"></param>
        /// <param name="overwrite">false, does not replace existing converter</param>
        /// <returns></returns>
        public bool Register(Type type, IStringConverter converter, bool overwrite = true) {
            if (_items.ContainsKey(type) && !overwrite) return false;
            _items[type] = converter;
            return true;
        }

        /// <summary>
        /// Register a new converter for a type
        /// </summary>
        /// <param name="converter"></param>
        /// <param name="overwrite">false, does not replace existing converter</param>
        /// <returns></returns>
        public bool Register<T>(IStringConverter<T> converter, bool overwrite = true) {
            var type = typeof(T);
            return Register(type, converter, overwrite);
        }

        /// <summary>
        /// Try to get IStringConverter<T>
        /// </summary>
        /// <param name="converter"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>true if found</returns>
        public bool TryGetConverter<T>(out IStringConverter<T> converter) {
            var type = typeof(T);
            if (_items.ContainsKey(type) && _items[type] is IStringConverter<T> c){
                converter = c;
                return true;
            }
            converter = StringConverterFactory.Empty<T>();
            return false;
        }

        /// <summary>
        /// Try to get IStringConverter
        /// </summary>
        /// <param name="converter"></param>
        /// <returns>true if found</returns>
        public bool TryGetConverter(Type type, out IStringConverter converter) {
            if (_items.ContainsKey(type) && _items[type] is IStringConverter c){
                converter = c;
                return true;
            }
            converter = StringConverterFactory.Empty(type);
            return false;
        }

        /// <summary>
        /// Get converter, throws exception if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IStringConverter<T> GetConverter<T>() {
            if (TryGetConverter<T>(out var converter)) return converter;
            throw ConvertException.From<T>();
        }

        /// <summary>
        /// Get converter, throws exception if not found
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IStringConverter GetConverter(Type type) {
            if (TryGetConverter(type, out var converter)) return converter;
            throw ConvertException.From(type);
        }

        public static readonly StringConverterProvider Global =
            new StringConverterProvider();
    }
}