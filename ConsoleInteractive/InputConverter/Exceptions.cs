using System;
namespace ConsoleInteractive.InputConverter
{
    public class ConvertException : Exception
    {
        private ConvertException(string message) : base(message) {}

        public static ConvertException From(Type t)
            => new ConvertException($"Can't find converter for {t}");
        public static ConvertException From<T>() => From(typeof(T));
    }
}