namespace ConsoleInteractive.Question.Validators.String
{
    public static class QuestionValidatorsExtensions
    {
        /// <summary>
        /// Add validator for length
        /// </summary>
        /// <param name="minLength">min required length</param>
        /// <returns></returns>
        public static QuestionValidators<string> AddMinLength(this QuestionValidators<string> q, int minLength) {
            return q.Add(e => e.Length >= minLength, $"Length must be greater or equal to {minLength}");
        }

        /// <summary>
        /// Add validator for length
        /// </summary>
        /// <param name="minLength">min required length</param>
        /// <returns></returns>
        public static QuestionValidators<string> AddMaxLength(this QuestionValidators<string> q, int maxLength) {
            return q.Add(e => e.Length <= maxLength, $"Length must be less or equal to {maxLength}");
        }
    }
}