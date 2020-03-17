using System.Linq;
using System.Threading.Tasks;

using System.Collections;
using System.Collections.Generic;
using System;

namespace ConsoleInteractive.InputValidation
{
    /// <summary>
    /// Collection of validators
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValidatorCollection<T> : IValidatorCollection<T>
    {
        private readonly List<Validator<T>> _items = new List<Validator<T>>();

        public IValidator<T> this[int index] => _items[index];
        IValidator IValidatorCollection.this[int index] => _items[index];
        public int Count => _items.Count;

        public ValidatorCollection<T> Add(Validator<T> fn) {
            _items.Add(fn);
            return this;
        }

        public ValidatorCollection<T> Add(Func<T, Task<(bool, string?)>> fn) {
            _items.Add(fn);
            return this;
        }

        public ValidatorCollection<T> Add(Func<T, (bool, string?)> fn) {
            _items.Add(fn);
            return this;
        }

        public ValidatorCollection<T> AddRange(IEnumerable<Validator<T>> fnList) {
            _items.AddRange(fnList);
            return this;
        }

        public ValidatorCollection<T> AddRange(IEnumerable<IValidator<T>> fnList) {
            _items.AddRange(fnList.Cast<Validator<T>>());
            return this;
        }

        public ValidatorCollection<T> AddRange(IEnumerable<Func<T, Task<(bool, string?)>>> fnList) {
            return AddRange(fnList.Cast<Validator<T>>());
        }

        public ValidatorCollection<T> AddRange(IEnumerable<Func<T, (bool, string?)>> fnList) {
            return AddRange(fnList.Cast<Validator<T>>());
        }

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
        public IEnumerator<IValidator> GetEnumerator() => _items.GetEnumerator();
        IEnumerator<IValidator<T>> IEnumerable<IValidator<T>>.GetEnumerator() => _items.GetEnumerator();

        public async Task<(bool, string?)> ValidateInput(T input)
        {
            try {
                var successCount = 0;
                foreach(var v in _items) {
                    var (success, msg) = await v.Validate(input);
                    if (!success) return (false, msg);
                    successCount++;
                }
                return (true, $"Passed {successCount} validations");
            } catch (ValidationException e) {
                return (false, e.Message);
            } catch (Exception e) {
                return (false, $"Error validating input: {e.Message}");
            }
        }

        public async Task<(bool, string?)> ValidateRawInput(object input)
        {
            try {
                var successCount = 0;
                foreach(IValidator v in _items) {
                    var (success, msg) = await v.RawValidate(input);
                    if (!success) return (false, msg);
                    successCount++;
                }
                return (true, $"Passed {successCount} validations");
            } catch (ValidationException e) {
                return (false, e.Message);
            } catch (Exception e) {
                return (false, $"Error validating input: {e.Message}");
            }
        }

        public IValidatorCollection MergeCollection(IValidatorCollection collection) => 
            ValidatorCollection.Create<T>().AddRange(this).AddRange(collection.Cast<Validator<T>>());

        public IValidatorCollection<T> MergeCollection(IValidatorCollection<T> collection) =>
            ValidatorCollection.Create<T>().AddRange(this).AddRange(collection);
    }

    public static class ValidatorCollection {
        /// <summary>
        /// Create new empty instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ValidatorCollection<T> Create<T>() {
            return new ValidatorCollection<T>();
        }
    }
}