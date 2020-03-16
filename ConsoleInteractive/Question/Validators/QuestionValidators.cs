using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleInteractive.Question.Validators
{
    public interface IQuestionValidators : IEnumerable {
        public Func<object, Task<(bool, string?)>> this[int index] { get; }
        public IQuestionValidators Merge(IQuestionValidators? validators);
    }

    public interface IQuestionValidators<T> : IEnumerable<Func<T, Task<(bool, string?)>>>, IEnumerable {
        public Func<T, Task<(bool, string?)>> this[int index] { get; }
    }

    /// <summary>
    /// List of validators for [T]
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuestionValidators<T> :  IQuestionValidators<T>, IQuestionValidators
    {
        private readonly List<Func<T, Task<(bool, string?)>>> _items =
            new List<Func<T, Task<(bool, string?)>>>();

        Func<object, Task<(bool, string?)>> IQuestionValidators.this[int index] { get {
            var fn = _items[index];
            return (object e) => fn((T)e);
        }}

        public Func<T, Task<(bool, string?)>> this[int index] => _items[index];

        /// <summary>
        /// Add a validator to the end
        /// </summary>
        public QuestionValidators<T> Add(Func<T, Task<(bool, string?)>> validator) {
            _items.Add(validator);
            return this;
        }
        /// <summary>
        /// Add a validator to the end
        /// </summary>
        public QuestionValidators<T> Add(Func<T, (bool, string?)> validator) {
            _items.Add(ConvertToAsync(validator));
            return this;
        }
        /// <summary>
        /// Add validator to end, generic message is generated if null
        /// </summary>
        public QuestionValidators<T> Add(Func<T, bool> validator, string? message = default) {
            var msg = message is null ? "" : message;
            return Add(e => (validator(e), msg));
        }
        /// <summary>
        /// Add validator to end, generic message is generated if null
        /// </summary>
        public QuestionValidators<T> Add(Func<T, Task<bool>> validator, string? message = default) {
            var msg = message is null ? "" : message;
            async Task<(bool, string?)> asyncValidator(T e) => (await validator(e), msg);
            return Add(asyncValidator);
        }

        /// <summary>
        /// Add a collection of values
        /// </summary>
        /// <returns></returns>
        public QuestionValidators<T> AddRange(IEnumerable<Func<T, Task<(bool, string?)>>> values) {
            _items.AddRange(values);
            return this;
        }

        /// <summary>
        /// Create a new list from a existing collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static QuestionValidators<T> From(IEnumerable<Func<T, Task<(bool, string?)>>> source) => 
            new QuestionValidators<T>().AddRange(source);

        /// <summary>
        /// Create a new empty collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static QuestionValidators<T> FromEmpty() => new QuestionValidators<T>();

        public IEnumerator<Func<T, Task<(bool, string?)>>> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
        /// <summary>
        /// Convert a Sync validator to a Async one
        /// </summary>
        /// <returns></returns>
        private Func<T, Task<(bool, string?)>> ConvertToAsync(Func<T, (bool, string?)> validator) {
            return (T e) => Task.Run(() => validator(e));
        }

        public IQuestionValidators Merge(IQuestionValidators? validators)
        {
            if (validators is null) return this;

            foreach (var v in validators) {
                if (v is Func<T, Task<(bool, string?)>> f) { Add(f); }
            }

            return this;
        }
    }
}