using System.Linq;
using System.Threading.Tasks;
using System;
using ConsoleInteractive.Selection;
using System.Collections.Generic;

namespace ConsoleInteractive
{
    public static class ConsoleI
    {
        /// <summary>
        /// Ask for confirmation, user must press 1 of the keys
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
        /// Select item from a group of options
        /// </summary>
        /// <param name="group">collection of options</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Select<T>(SelectionGroup<T> group) {
            return SelectionGroup.Select(group, 1).First();
        }

        /// <summary>
        /// Select items from a group of options
        /// </summary>
        /// <param name="group">collection of options</param>
        /// <param name="max">max allowed selections</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Select<T>(SelectionGroup<T> group, int max) {
            return SelectionGroup.Select(group, max);
        }

        /// <summary>
        /// Select a item from an enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Select<T>() where T : Enum {
            return SelectionGroup.Select(SelectionGroup.FromEnum<T>()).First();
        }

        /// <summary>
        /// Select a items from an enum
        /// </summary>
        /// <param name="max">max allowed selections</param>
        /// <returns></returns>
        public static IEnumerable<T> Select<T>(int max) where T : struct, Enum {
            return SelectionGroup.Select(SelectionGroup.FromEnum<T>(), max);
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
    }
}
