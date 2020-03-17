using System.Threading.Tasks;
using System;

namespace ConsoleInteractive.InputValidation
{
    /// <summary>
    /// Validates a input
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validator target
        /// </summary>
        Type Target { get; }

        /// <summary>
        /// Function validator
        /// </summary>
        /// <returns>Tuple with validation result</returns>
        Func<object, Task<(bool, string?)>> RawValidate { get; }
    }

    /// <summary>
    /// Validates a input
    /// </summary>
    public interface IValidator<T> : IValidator
    {
        /// <summary>
        /// Function validator
        /// </summary>
        /// <returns>Tuple with validation result</returns>
        Func<T, Task<(bool, string?)>> Validate { get; }
    }
}