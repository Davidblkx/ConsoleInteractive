using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleInteractive.Question
{
    public class QuestionFactory<T>
    {
        private readonly BaseQuestion<T> _question;

        private QuestionFactory(BaseQuestion<T> question) {
            _question = question;
        }

        /// <summary>
        /// Ask question and wait validation
        /// </summary>
        /// <returns></returns>
        public Task<T> AskQuestion() {
            return AskQuestion(
                _question.QuestionMessage,
                _question.DefaultValue,
                _question.Validators
            );
        }

        /// <summary>
        /// Ask a question and validate response
        /// </summary>
        /// <param name="question">message in question</param>
        /// <param name="defaultValue">Default value to return</param>
        /// <returns></returns>
        public Task<T> AskQuestion(string question, T defaultValue = default) {
            var defValue = defaultValue ?? _question.DefaultValue;
            return AskQuestion(question, defValue, _question.Validators);
        }

        /// <summary>
        /// Ask a question and validate response
        /// </summary>
        /// <param name="question">message in question</param>
        /// <param name="defaultValue">Default value to return</param>
        /// <param name="IEnumerable<Func<string">Validators to parse response</param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<T> AskQuestion(
            string question, 
            T defaultValue,
            IEnumerable<Func<string, Task<(bool success, string? message)>>> validators
        ) {
            // Buffer name to memorize
            const string BUFFER_NAME = "#QUESTION#";

            // Build question -> SOMETHING? (DEFAULT)
            var toAsk = BuildQuestionToAsk(question, defaultValue);
            Console.WriteLine(toAsk);

            // Save curent buffer position
            ConsoleBuffer.MemoriseBufferPosition(BUFFER_NAME);

            do { // Keep asking use, until valid response
                try {
                    var res = Console.ReadLine();

                    // Set default value if empty
                    if (string.IsNullOrEmpty(res)) 
                    { res = _question.ConvertToString(defaultValue); }

                    // Run all validators
                    foreach (var fn in validators) {
                        var (success, message) = await fn(res);
                        if (!success) { throw new Exception(message ?? "Validator failed"); }
                    }

                    // Convert to Type [T]
                    return _question.ConvertFromString(res);
                } catch (Exception e) { // Catch all exceptions
                    // Write to console
                    Console.WriteLine(e.Message + ",\nPress [Return] to retry...");

                    // Wait user confirmation
                    Console.ReadKey();

                    // Clear input and return to question
                    ConsoleBuffer.ClearBufferFrom(BUFFER_NAME);
                }
            } while(true);
        }

        public static QuestionFactory<T> Create(BaseQuestion<T> question) {
            return new QuestionFactory<T>(question);
        }

        private string BuildQuestionToAsk(string question, T defaultValue) {
            var defVal = _question.ConvertToString(defaultValue);
            var defValToShow = string.IsNullOrEmpty(defVal) ? "" : $"({defVal})";
           return $"{question}? {defValToShow}";
        }
    }
}