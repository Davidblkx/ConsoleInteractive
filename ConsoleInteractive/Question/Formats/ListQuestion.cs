using System.Linq;
using System.Collections.Generic;
namespace ConsoleInteractive.Question.Formats
{
    public class ListQuestion<T> : BaseQuestion<List<T>>
    {
        public ListQuestion() : base("", new List<T>()) { }

        public override List<T> ConvertFromString(string value) {
            var listValues = value.Split(';');
            var baseQuestion = QuestionFactoryProvider.GetQuestionFactory<T>();
            var list = new List<T>();

            foreach (var i in listValues) {
                list.Add(baseQuestion.Question.ConvertFromString(i));
            }

            return list;
        }

        public override string ConvertToString(List<T> value) {
            var baseQuestion = QuestionFactoryProvider.GetQuestionFactory<T>();
            return string.Join(";", value.Select(e => baseQuestion.Question.ConvertToString(e)));
        }
    }
}