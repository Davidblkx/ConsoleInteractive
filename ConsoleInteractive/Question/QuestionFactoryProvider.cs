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
        private static readonly Dictionary<Type, object> _factories = 
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
        /// Get factory for a type, throws if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQuestionFactory GetQuestionFactory(Type type) {
            if (
                !_factories.ContainsKey(type) ||
                !(_factories[type] is IQuestionFactory f)
            ) { throw new Exception("Provider not found for type " + type.ToString()); }
            
            return f;
        }

        /// <summary>
        /// Load build in providers
        /// </summary>
        public static void RegisterDefaultProviders() {
            RegisterQuestionFormat(new StringQuestion());
            RegisterQuestionFormat(new LongQuestion());
            RegisterQuestionFormat(new IntQuestion());
            RegisterQuestionFormat(new ULongQuestion());
            RegisterQuestionFormat(new UIntQuestion());
            RegisterQuestionFormat(new DoubleQuestion());
            RegisterQuestionFormat(new FloatQuestion());
            RegisterQuestionFormat(new DecimalQuestion());
            RegisterQuestionFormat(new ListQuestion<string>());
            RegisterQuestionFormat(new ListQuestion<long>());
            RegisterQuestionFormat(new ListQuestion<int>());
            RegisterQuestionFormat(new ListQuestion<ulong>());
            RegisterQuestionFormat(new ListQuestion<uint>());
            RegisterQuestionFormat(new ListQuestion<double>());
            RegisterQuestionFormat(new ListQuestion<float>());
            RegisterQuestionFormat(new ListQuestion<decimal>());
        }
    }
}