using System.Linq;
using System.Threading.Tasks;
using System;
using ConsoleInteractive.Question;
using ConsoleInteractive.Question.Validators;
using ConsoleInteractive.Selection;
using System.Collections.Generic;

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
        /// Select item from a group of options
        /// </summary>
        /// <param name="group">collection of options</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Select<T>(SelectionGroup<T> group) {
            return SelectionGroup.Select(group, 1).First();
        }

        /// <summary>
        /// Select items from a group of options
        /// </summary>
        /// <param name="group">collection of options</param>
        /// <param name="max">max allowed selections</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Select<T>(SelectionGroup<T> group, int max) {
            return SelectionGroup.Select(group, max);
        }

        /// <summary>
        /// Select a item from an enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Select<T>() where T : Enum {
            return SelectionGroup.Select(SelectionGroup.FromEnum<T>()).First();
        }

        /// <summary>
        /// Select a items from an enum
        /// </summary>
        /// <param name="max">max allowed selections</param>
        /// <returns></returns>
        public static IEnumerable<T> Select<T>(int max) where T : struct, Enum {
            return SelectionGroup.Select(SelectionGroup.FromEnum<T>(), max);
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
