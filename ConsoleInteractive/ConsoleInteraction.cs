using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using ConsoleInteractive.Components;
using ConsoleInteractive.Form;
using ConsoleInteractive.InputValidation;
using ConsoleInteractive.InputConverter;

namespace ConsoleInteractive
{
    public static class ConsoleI
    {
        /// <summary>
        /// Ask for confirmation, user must press one of the keys
        /// </summary>
        /// <param name="message"></param>
        /// <param name="okKey"></param>
        /// <param name="koKey"></param>
        /// <returns></returns>
        public static bool AskConfirmation(string message, ConsoleKey okKey = ConsoleKey.Y, ConsoleKey koKey = ConsoleKey.N) {
            const string BUFFER_KEY = "INTERNAL_BUFFER#CONFIRMATION";

            ConsoleBuffer.MemoriseBufferPosition(BUFFER_KEY);

            do {
                Console.WriteLine($"{message}? [{okKey}/{koKey}]");
                var key = Console.ReadKey();

                if (key.Key == okKey) { return true; }
                if (key.Key == koKey) { return false; }
                ConsoleBuffer.ClearBufferFrom(BUFFER_KEY);
            } while(true);
        }

        /// <summary>
        /// Render form for type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<T> RenderForm<T>() where T : new() {
            return ConsoleForm
                .BuildForm<T>()
                .Request();
        }

        /// <summary>
        /// Show message, request for input and use the default validator store to validate result
        /// </summary>
        /// <param name="message"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<T> Ask<T>(string message, T defaultValue = default, IValidatorCollection<T>? validators = null, StringConverterProvider? provider = null) {
            return InputText.Create<T>(
                message, defaultValue, validators, provider)
                .RequestInput();
        }

        /// <summary>
        /// Select item from a group of options
        /// </summary>
        /// <param name="group">collection of options</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> Select<T>(IEnumerable<T> options) {
            return (await InputSelection.From<T>()
                .AddOption(options)
                .RequestInput()
            ).First();
        }

        /// <summary>
        /// Select items from a group of options
        /// </summary>
        /// <param name="group">collection of options</param>
        /// <param name="max">max allowed selections</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Task<IEnumerable<T>> Select<T>(IEnumerable<T> options, int max) {
            return InputSelection.From<T>()
                .SetMaxSelected(max)
                .AddOption(options)
                .RequestInput();
        }

        /// <summary>
        /// Select a item from an enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> Select<T>() where T : Enum {
            return (
                await InputSelection
                    .FromEnum<T>()
                    .RequestInput()
            ).First();
        }

        /// <summary>
        /// Select a items from an enum
        /// </summary>
        /// <param name="max">max allowed selections</param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> Select<T>(int max) where T : Enum {
            return (
                InputSelection
                    .FromEnum<T>()
                    .SetMaxSelected(max)
                    .RequestInput()
            );
        }

        /// <summary>
        /// Show message to user and await for ANY KEY PRESS
        /// </summary>
        /// <param name="message"></param>
        public static void AwaitContinue(string? message = default) {
            var m = message ?? "Press [RETURN] to continue...";
            Console.WriteLine(m);
            Console.ReadKey();
        }

        /// <summary>
        /// Register defaults for global providers
        /// </summary>
        public static void RegisterDefaults() {
            DefaultRenderProviders.Register();
        }
    }
}
