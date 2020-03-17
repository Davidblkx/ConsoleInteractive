using System;
using System.Threading.Tasks;

namespace ConsoleInteractive.InputValidation
{
    /// <summary>
    /// Implements validation for type [T]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Validator<T> : IValidator<T>
    {
        /// <summary>
        /// Type of validation target
        /// </summary>
        /// <returns></returns>
        public Type Target => typeof(T);

        public Func<T, Task<(bool, string?)>> Validate { get; }
        public Func<object, Task<(bool, string?)>> RawValidate { get; }

        public Validator(Func<T, Task<(bool, string?)>> validatorFn) {
            Validate = validatorFn;
            RawValidate = BuildObjectValidator(validatorFn);
        }

        public Validator(Func<T, (bool, string?)> validatorFn) {
            Validate = BuildAsyncValidator(validatorFn);
            RawValidate = BuildObjectValidator(Validate);
        }

        public static implicit operator Validator<T>(Func<T, Task<(bool, string?)>> fn) => new Validator<T>(fn);
        public static implicit operator Validator<T>(Func<T, (bool, string?)> fn) => new Validator<T>(fn);

        public static Validator<T> From(IValidator<T> v) => new Validator<T>(v.Validate);

        private static Func<object, Task<(bool, string?)>> BuildObjectValidator(
            Func<T, Task<(bool, string?)>> validatorFn) {
                return (object e) => e is T t ? validatorFn(t) : 
                    throw new Exception("Validation not meant for type " + typeof(T));
        }

        private static Func<T, Task<(bool, string?)>> BuildAsyncValidator(
            Func<T, (bool, string?)> fn) {
                return (T e) => Task.Run(() => fn(e));
        }
    }
}