using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleInteractive.Question.Validators
{
    public class QuestionValidatorBuilder
    {
        private readonly List<Func<string, Task<(bool success, string? message)>>> _validators =
            new List<Func<string, Task<(bool success, string? message)>>>();

        public IEnumerable<Func<string, Task<(bool success, string? message)>>> Validators => _validators;

        /// <summary>
        /// Add range of validators
        /// </summary>
        /// <returns></returns>
        public QuestionValidatorBuilder AddRange(IEnumerable<Func<string, (bool success, string? message)>> validators) {
            _validators.AddRange(validators.Select(v => FromSync(v)));
            return this;
        }

        /// <summary>
        /// Add range of Async validators
        /// </summary>
        /// <returns></returns>
        public QuestionValidatorBuilder AddAsyncRange(IEnumerable<Func<string, Task<(bool success, string? message)>>> validators) {
            _validators.AddRange(validators);
            return this;
        }

        /// <summary>
        /// Add a new validator
        /// </summary>
        /// <returns></returns>
        public QuestionValidatorBuilder Add(Func<string, (bool success, string? message)> validator) {
            _validators.Add(FromSync(validator));
            return this;
        }

        /// <summary>
        /// Add a new Async validator
        /// </summary>
        /// <returns></returns>
        public QuestionValidatorBuilder AddAsync(Func<string, Task<(bool success, string? message)>> validator) {
            _validators.Add(validator);
            return this;
        }

        public static QuestionValidatorBuilder Create() {
            return new QuestionValidatorBuilder();
        }

        /// <summary>
        /// Create builder from List of validators
        /// </summary>
        /// <returns></returns>
        public static QuestionValidatorBuilder FromList(
            IEnumerable<Func<string, Task<(bool success, string? message)>>> validators) {
            var builder = new QuestionValidatorBuilder();
            builder._validators.AddRange(validators);
            return builder;
        }

        /// <summary>
        /// Create a Async Validator from a Sync one
        /// </summary>
        /// <returns></returns>
        public static Func<string, Task<(bool success, string? message)>> FromSync(
            Func<string, (bool success, string? message)> action) => 
                e => Task.Run(() => action(e));
    }
}