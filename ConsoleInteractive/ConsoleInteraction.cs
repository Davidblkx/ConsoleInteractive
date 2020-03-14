using System.Threading.Tasks;
using System;
using ConsoleInteractive.Question;
using ConsoleInteractive.Question.Validators;

namespace ConsoleInteractive
{
    public static class ConsoleI
    {
        /// <summary>
        /// Ask a question and validate response
        /// </summary>
        /// <param name="questionMessage"></param>
        /// <param name="defaultValue"></param>
        /// <param name="IEnumerable<Func<string"></param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<T> AskQuestion<T>(
            string questionMessage,
            T defaultValue,
            IQuestionValidators<T> validators
        ) {
            return QuestionFactoryProvider
                .GetQuestionFactory<T>()
                .AskQuestion(questionMessage, defaultValue, validators);
        }

        /// <summary>
        /// Ask a question and validate response
        /// </summary>
        /// <param name="questionMessage">message in question</param>
        /// <param name="defaultValue">Default value to return</param>
        /// <returns></returns>
        public static Task<T> AskQuestion<T>(string questionMessage, T defaultValue = default) {
            return QuestionFactoryProvider
                .GetQuestionFactory<T>()
                .AskQuestion(questionMessage, defaultValue);
        }

        /// <summary>
        /// Show message to user and await for ANY KEY PRESS
        /// </summary>
        /// <param name="message"></param>
        public static void AwaitContinue(string? message = default) {
            var m = message ?? "Press [RETURN] to continue...";
            Console.WriteLine(m);
            Console.ReadKey();
        }
    }
}
