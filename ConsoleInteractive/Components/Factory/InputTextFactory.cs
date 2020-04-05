using ConsoleInteractive.InputValidation;
using ConsoleInteractive.InputConverter;

namespace ConsoleInteractive.Components.Factory
{
    public static class InputTextFactory
    {
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