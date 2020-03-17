using System;

namespace ConsoleInteractive.InputValidation
{
    /// <summary>
    /// Exception to throw when doing a validation, same as returning (false, "")
    /// </summary>
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) {}
    }
}