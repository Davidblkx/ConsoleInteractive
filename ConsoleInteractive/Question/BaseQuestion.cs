using System;
using ConsoleInteractive.Question.Validators;

namespace ConsoleInteractive.Question
{
    public interface IBaseQuestion
    {
        string QuestionMessage { get; }
        object? DefaultValue { get; }

        object? ConvertFromString(string value);
        string ConvertToString(object value);

        IQuestionValidators Validators { get; }
    }

    /// <summary>
    /// Base class to be extended by question types
    /// </summary>
    public abstract class BaseQuestion<T> : IBaseQuestion
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
        object? IBaseQuestion.DefaultValue => DefaultValue;

        /// <summary>
        /// Validators to validate response
        /// </summary>
        /// <param name="success">true, if passed with success</param>
        /// <param name="message">message to alert user of error</param>
        /// <returns></returns>
        public QuestionValidators<T> Validators => QuestionValidators<T>.FromEmpty();
        IQuestionValidators IBaseQuestion.Validators => Validators;

        /// <summary>
        /// Value type
        /// </summary>
        /// <returns></returns>
        public Type ValueType => typeof(T);

        /// <param name="questionMessage">Question to ask</param>
        public BaseQuestion(string questionMessage, T defaultValue)
        {
            QuestionMessage = questionMessage;
            DefaultValue = defaultValue;
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

        object? IBaseQuestion.ConvertFromString(string value) => ConvertFromString(value);
        public string ConvertToString(object value) => ConvertToString(value);
    }
}