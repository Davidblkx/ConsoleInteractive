using System.Threading.Tasks;
using ConsoleInteractive.InputConverter;
using ConsoleInteractive.InputValidation;

namespace ConsoleInteractive.InputRender
{
    /// <summary>
    /// Base class for components
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseInputRender<T> : IRender<T>
    {
        /// <summary>
        /// Provider for current context, Global by default
        /// </summary>
        /// <value></value>
        public StringConverterProvider ConverterProvider { get; set; }
            = StringConverterProvider.Global;

        /// <summary>
        /// Collection of validators
        /// </summary>
        /// <value></value>
        public IValidatorCollection<T> Validators { get; set; }
            = new ValidatorCollection<T>();

        /// <summary>
        /// Method to render and request data
        /// </summary>
        /// <returns></returns>
        public abstract Task<T> RequestInput();
        async Task<object?> IRender.RequestInput() => await RequestInput();

        /// <summary>
        /// Returns converter for current type
        /// </summary>
        /// <returns></returns>
        protected IStringConverter<T> GetConverter() {
            return StringConverterFactory.Create<T>(ConverterProvider);
        }
    }
}