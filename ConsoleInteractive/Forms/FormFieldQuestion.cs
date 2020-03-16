using System.Threading.Tasks;
using System.Reflection;
using ConsoleInteractive.Question.Validators;
using ConsoleInteractive.Question;

namespace ConsoleInteractive.Forms
{
    public class FormFieldQuestion
    {
        public string Message { get; set; }

        public IQuestionValidators Validators { get; set; }

        public object? DefaultValue { get; set; }

        public IQuestionFactory QuestionFactory { get; private set; }

        public Task<object?> GetValue() {
            return QuestionFactory.AskQuestion(Message, DefaultValue, Validators);
        }

        private FormFieldQuestion(string message, object? defaultValue, IQuestionValidators validators, IQuestionFactory factory) {
            Message = message;
            DefaultValue = defaultValue;
            Validators = validators;
            QuestionFactory = factory;
        }
        
        public static FormFieldQuestion FromPropertyInfo(PropertyInfo info, FormField fieldAttr) {
            var factory = QuestionFactoryProvider.GetQuestionFactory(info.PropertyType);

            var msg = string.IsNullOrEmpty(fieldAttr.Message) ? 
                factory.Question.QuestionMessage : fieldAttr.Message;
            if (string.IsNullOrEmpty(msg))
                msg = $"Value for {info.Name}";

            var defValue = fieldAttr.DefaultValue ?? factory.Question.DefaultValue;
            var validators = factory.Question.Validators;

            return new FormFieldQuestion(msg, defValue, validators, factory);
        }
    }
}