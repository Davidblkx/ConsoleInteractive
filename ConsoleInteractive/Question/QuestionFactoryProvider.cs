using System;
using System.Collections.Generic;
using ConsoleInteractive.Question.Formats;

namespace ConsoleInteractive.Question
{
    /// <summary>
    /// Provide factory
    /// </summary>
    public static class QuestionFactoryProvider
    {
        private static readonly Dictionary<Type, Object> _factories = 
            new Dictionary<Type, object>();

        /// <summary>
        /// Register a question factory for a type
        /// </summary>
        /// <param name="question"></param>
        /// <typeparam name="T"></typeparam>
        public static void RegisterQuestionFormat<T>(BaseQuestion<T> question) {
            var factory = QuestionFactory<T>.Create(question);
            _factories[question.ValueType] = factory;
        }

        /// <summary>
        /// Try to get factory for a type, null if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static QuestionFactory<T>? TryGetQuestionFactory<T>() {
            var type = typeof(T);

            if (
                !_factories.ContainsKey(type) ||
                !(_factories[type] is QuestionFactory<T> f)
            ) { return null; }
            
            return f;
        }

        /// <summary>
        /// Get factory for a type, throws if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static QuestionFactory<T> GetQuestionFactory<T>() {
            var type = typeof(T);

            if (
                !_factories.ContainsKey(type) ||
                !(_factories[type] is QuestionFactory<T> f)
            ) { throw new Exception("Provider not found for type " + typeof(T).ToString()); }
            
            return f;
        }

        /// <summary>
        /// Load build in providers
        /// </summary>
        public static void RegisterDefaultProviders() {
            RegisterQuestionFormat(new StringQuestion());
        }
    }
}