using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleInteractive.Question.Formats
{
    public class StringQuestion : BaseQuestion<string>
    {
        public StringQuestion(): base("Something", "") {}

        public override string ConvertFromString(string value) => value;
        public override string ConvertToString(string value) => value;

        /// <summary>
        /// Create question with custom validators
        /// </summary>
        /// <param name="IEnumerable<Func<string"></param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static StringQuestion FromValidators(
            IEnumerable<Func<string, Task<(bool success, string? message)>>> validators
        ) {
            var q = new StringQuestion();
            q.AddValidators(validators);
            return q;
        }
    }
}