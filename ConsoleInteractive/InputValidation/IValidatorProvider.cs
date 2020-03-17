using System.Threading.Tasks;

namespace ConsoleInteractive.InputValidation
{
    public interface IValidatorProvider
    {
        Task<(bool, string?)> Validate(string name, object value);
    }
}