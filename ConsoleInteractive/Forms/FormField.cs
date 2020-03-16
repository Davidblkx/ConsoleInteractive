using System;
using ConsoleInteractive.Question.Validators;

namespace ConsoleInteractive.Forms
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FormField : Attribute
    {
        public string Message { get; set; } = "";

        public object? DefaultValue { get; set; }

        public FormField() {}

        public FormField(string message, object? defaultValue = default) {
            Message = message;
            DefaultValue = defaultValue;
        }
    }
}