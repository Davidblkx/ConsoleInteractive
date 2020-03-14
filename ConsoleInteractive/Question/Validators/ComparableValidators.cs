using System;

namespace ConsoleInteractive.Question.Validators.Comparable
{
    public static class ComparableValidators
    {
        public static QuestionValidators<T> GreaterThan<T>(this QuestionValidators<T> q, T value) 
            where T : IComparable => q.Add(e => e.CompareTo(value) > 0, $"Must be greater than {value}");

        public static QuestionValidators<T> GreaterThanOrEqual<T>(this QuestionValidators<T> q, T value) 
            where T : IComparable => q.Add(e => e.CompareTo(value) >= 0, $"Must be greater than or equal to {value}");

        public static QuestionValidators<T> Equal<T>(this QuestionValidators<T> q, T value) 
            where T : IComparable => q.Add(e => e.CompareTo(value) == 0, $"Must be equal to {value}");

        public static QuestionValidators<T> LessThan<T>(this QuestionValidators<T> q, T value) 
            where T : IComparable => q.Add(e => e.CompareTo(value) < 0, $"Must be less than {value}");

        public static QuestionValidators<T> LessThanOrEqual<T>(this QuestionValidators<T> q, T value) 
            where T : IComparable => q.Add(e => e.CompareTo(value) <= 0, $"Must be less than or equal to {value}");
    }
}