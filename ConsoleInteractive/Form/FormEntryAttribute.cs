using System;
namespace ConsoleInteractive.Form
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FormEntry : Attribute
    {
        /// <summary>
        /// Key to get render from Global components provider
        /// if null, will try to use typeof property
        /// </summary>
        /// <value></value>
        public string? ProviderKey { get; set; }

        /// <summary>
        /// Message to show
        /// </summary>
        /// <value></value>
        public string? Message { get; set; }

        /// <summary>
        /// Key to get validators for property
        /// </summary>
        /// <value></value>
        public string? ValidatorsKey { get; set; }

        /// <summary>
        /// Used to define the priority of the question, 0 is the highest priority
        /// </summary>
        /// <value></value>
        public ulong Priority { get; set; } = ulong.MaxValue;

        public FormEntry() { }
    }
}