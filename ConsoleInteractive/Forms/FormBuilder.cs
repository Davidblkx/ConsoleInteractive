using System.Threading.Tasks;
namespace ConsoleInteractive.Forms
{
    public static class FormBuilder
    {
        public static async Task<T> RequestForm<T>() where T : class, new() {
            var obj = new T();

            foreach(var info in typeof(T).GetProperties()) {
                foreach(var attr in info.GetCustomAttributes(true)) {
                    if (attr is FormField field) {
                        var question = FormFieldQuestion.FromPropertyInfo(info, field);
                        var value = await question.GetValue();
                        if (value is null) continue;
                        
                        info.SetValue(obj, value);
                        break;
                    }
                }
            }

            return obj;
        }
    }
}