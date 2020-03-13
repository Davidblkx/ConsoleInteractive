using System;
using System.Threading.Tasks;

namespace ConsoleInteractive.Question.Validators
{
    public static class QuestionValidators
    {
        /// <summary>
        /// Create validator for min length
        /// </summary>
        /// <returns></returns>
        public static Func<string, Task<(bool success, string? message)>> MinLength(int length) {
            return QuestionValidatorBuilder.FromSync(e => 
                (e.Length >= length, $"Input should have length greater than {length}"));
        }

        /// <summary>
        /// Create validator for max length
        /// </summary>
        /// <returns></returns>
        public static Func<string, Task<(bool success, string? message)>> MaxLength(int length) {
            return QuestionValidatorBuilder.FromSync(e => 
                (e.Length <= length, $"Input should have length smaller than {length}"));
        }

        /// <summary>
        /// Create validator for not empty
        /// </summary>
        /// <returns></returns>
        public static Func<string, Task<(bool success, string? message)>> NotEmpty() {
            return QuestionValidatorBuilder.FromSync(e => 
                (!string.IsNullOrEmpty(e), "Input can't be empty"));
        }
    }
}