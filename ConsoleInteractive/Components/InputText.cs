using System;
using System.Threading.Tasks;
using ConsoleInteractive.InputConverter;
using ConsoleInteractive.InputRender;
using ConsoleInteractive.InputValidation;

namespace ConsoleInteractive.Components
{
    /// <summary>
    /// Request a text input
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InputText<T> : BaseInputRender<T>
    {
        public Type Target => typeof(T);

        /// <summary>
        /// Default value to use if no input
        /// </summary>
        /// <value></value>
        public object? DefaultValue { get; set; }

        public InputText() {
            Message = $"Input value for a {Target}";
        }

        public override async Task<T> RequestInput()
        {
            const string BUFFER_NAME = "#INTERNAL.COMPONENT.TEXTINPUT.TOP";
            // Write message with default value
            Console.WriteLine(BuildMessage());
            // Save current console position
            ConsoleBuffer.MemoriseBufferPosition(BUFFER_NAME);

            while (true) {
                try {
                    var res = Console.ReadLine();
                    // Convert response to type, or get default value if empty
                    var data = GetDataFromResponse(res);
                    // Run validators
                    var (valid, message) = await Validators.ValidateInput(data);
                    // If not valid
                    if (!valid) throw new Exception(message ?? "Error validating input");
                    // return converted input
                    return data;
                } catch (Exception ex) {
                    // Write to console
                    Console.WriteLine(ex.Message + ",\nPress [Return] to retry...");
                    // Wait user confirmation
                    Console.ReadKey();
                    // Clear input and return to question
                    ConsoleBuffer.ClearBufferFrom(BUFFER_NAME);
                }
            }
        }

        private T GetDataFromResponse(string? response) {
            T ToObject(string v) => GetConverter().ToObject(v);
            return (string.IsNullOrEmpty(response), DefaultValue is null) switch
            {
                (false, _) => ToObject(response!), // not empty
                (true, false) => (T)DefaultValue!, //empty but has default value
                (_, _) => ToObject("") // empty, without default value
            };
        }

        private string BuildMessage() {
            var defaultVal = GetDefaultValueString();
            if (!string.IsNullOrEmpty(defaultVal))
                defaultVal = $"({defaultVal})";
            return $"{Message} {defaultVal}";
        }

        private string GetDefaultValueString() {
            if (DefaultValue is null) return "";
            var converter = GetConverter();
            return converter.ToString(DefaultValue);
        }

        public static InputText<T> Create(
            string message, 
            T defaultValue = default, 
            IValidatorCollection<T>? validators = default, 
            StringConverterProvider? provider = default) {
            return new InputText<T> {
                Message = message,
                DefaultValue = defaultValue,
                Validators = validators ?? ValidatorCollection.Create<T>(),
                ConverterProvider = provider ?? StringConverterProvider.Global
            };
        }
    }

    public static class InputText {
        public static InputText<T> Create<T>(
            string message, 
            T defaultValue = default, 
            IValidatorCollection<T>? validators = default, 
            StringConverterProvider? provider = default) {
            return new InputText<T> {
                Message = message,
                DefaultValue = defaultValue,
                Validators = validators ?? ValidatorCollection.Create<T>(),
                ConverterProvider = provider ?? StringConverterProvider.Global
            };
        }
    }
}