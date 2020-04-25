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

    /// <summary>
    /// Throw inside StringConverter to request other input
    /// </summary>
    public class ConvertStringFormatException : Exception
    {
        public ConvertStringFormatException()
            : base("Error converting string to type") {}

        public ConvertStringFormatException(string message)
            : base(message) {}
    }
}