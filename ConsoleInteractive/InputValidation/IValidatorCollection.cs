using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
namespace ConsoleInteractive.InputValidation
{
    /// <summary>
    /// Collection of validations for a type
    /// </summary>
    public interface IValidatorCollection : IEnumerable<IValidator>, IEnumerable
    {
        IValidator this[int index] { get; }
        int Count { get; }

        /// <summary>
        /// Validate input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<(bool, string?)> ValidateRawInput(object input);
        
        /// <summary>
        /// return a new collection that Merge multiple collections
        /// </summary>
        /// <param name="collection"></param>
        IValidatorCollection MergeCollection(IValidatorCollection collection);
    }

    /// <summary>
    /// Collection of validations for a type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidatorCollection<T> : IValidatorCollection, IEnumerable<IValidator<T>>
    {
        new IValidator<T> this[int index] { get; }

        /// <summary>
        /// Validate input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<(bool, string?)> ValidateInput(T input);

        /// <summary>
        /// Return a new collection that Merge multiple collections
        /// </summary>
        /// <param name="collection"></param>
        IValidatorCollection<T> MergeCollection(IValidatorCollection<T> collection);
    }
}