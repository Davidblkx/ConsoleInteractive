using System.Collections.Generic;

namespace ConsoleInteractive.InputValidation
{
    public static class Extensions
    {
        /// <summary>
        /// Convert list to ValidatorCollection
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IValidatorCollection<T> ToValidatorCollection<T>(this IEnumerable<IValidator<T>> list) {
            return new ValidatorCollection<T>().AddRange(list);
        }
    }
}