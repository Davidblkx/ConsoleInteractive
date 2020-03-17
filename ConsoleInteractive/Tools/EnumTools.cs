using System.Collections.Generic;
using System;
using System.Linq;

namespace ConsoleInteractive.Tools
{
    public static class EnumTools
    {
        /// <summary>
        /// Checks if enum has flags attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool HasFlagsAttribute<T>() where T : Enum {
            return typeof(T).GetCustomAttributes(true).Any(e => e is FlagsAttribute);
        }

        /// <summary>
        /// Get values assigned to Enum value, only work with Flag enums
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetActiveValues<T>(T value) where T : Enum {
            if (!HasFlagsAttribute<T>()) return new List<T> { value };
            var list = new List<T>();

            foreach (var e in Enum.GetValues(typeof(T)).Cast<T>()) {
                if (value.HasFlag(e)) list.Add(e);
            }

            return list;
        }
    }
}