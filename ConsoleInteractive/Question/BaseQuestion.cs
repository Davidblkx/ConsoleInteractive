using System.Threading.Tasks;
using System;
using System.Collections.Generic;
namespace ConsoleInteractive.Question
{
    /// <summary>
    /// Base class to be extended by question types
    /// </summary>
    public abstract class BaseQuestion<T>
    {
        /// <summary>
        /// Question to ask
        /// </summary>
        public string QuestionMessage { get; set; }

        /// <summary>
        /// Default value to use if response is empty
        /// </summary>
        /// <value></value>
        public T DefaultValue { get; set; }

        /// <summary>
        /// Validators to validate response
        /// </summary>
        /// <param name="success">true, if passed with success</param>
        /// <param name="message">message to alert user of error</param>
        /// <returns></returns>
        public IEnumerable<Func<string, Task<(bool success, string? message)>>> Validators => _validators;
            
        private readonly List<Func<string, Task<(bool success, string? message)>>> _validators = 
            new List<Func<string, Task<(bool success, string? message)>>>();

        /// <summary>
        /// Value type
        /// </summary>
        /// <returns></returns>
        public Type ValueType => typeof(T);

        /// <param name="questionMessage">Question to ask</param>
        public BaseQuestion(string questionMessage, T defaultValue) {
            QuestionMessage = questionMessage;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Add new validator
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public void AddValidator(Func<string, Task<(bool success, string? message)>> validator) {
            _validators.Add(validator);
        }

        /// <summary>
        /// Add multiple validators
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public void AddValidators(IEnumerable<Func<string, Task<(bool success, string? message)>>> validators) {
            _validators.AddRange(validators);
        }

        /// <summary>
        /// Convert value to string
        /// 
        /// Used by Factory, when displaying the default value
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public virtual string ConvertToString(T value) => 
            Convert.ChangeType(value, typeof(string)) as string ?? "";

        /// <summary>
        /// Convert response to expected value type
        /// </summary>
        /// <param name="vale"></param>
        /// <returns></returns>
        public virtual T ConvertFromString(string value) =>
            (T)Convert.ChangeType(value, typeof(T)) ??
                throw new Exception("Can't convert type " + typeof(T).ToString());
    }
}