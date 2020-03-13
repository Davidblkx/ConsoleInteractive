using System;
using System.Collections.Generic;
namespace ConsoleInteractive
{
    public static class ConsoleBuffer
    {
        private static readonly Dictionary<string, (int x, int y)> _memo =
            new Dictionary<string, (int x, int y)>();

        /// <summary>
        /// Memorise a buffer position
        /// </summary>
        /// <param name="name"></param>
        public static void MemoriseBufferPosition(string name) {
            _memo[name] = (Console.CursorLeft, Console.CursorTop);
        }

        /// <summary>
        /// Forget a buffer position
        /// </summary>
        /// <param name="name"></param>
        public static void ForgetBufferPosition(string name) {
            _memo.Remove(name);
        }

        /// <summary>
        /// Replace buffer with a char
        /// </summary>
        /// <param name="name">name of buffer memory</param>
        /// <param name="value">value to replace with, space by default</param>
        /// <param name="left">replace until this column, current by default</param>
        /// <param name="top">replace until this line, current by default</param>
        public static void ReplaceBufferFrom(string name, char value = ' ', int? left = default, int? top = default) {
            if (!_memo.ContainsKey(name)) { return; }

            var (x, y) = _memo[name];

            var xTarget = left ?? Console.CursorLeft;
            var yTarget = top ?? Console.CursorTop;
            
            if (xTarget < x || yTarget < y) { return; }

            Console.SetCursorPosition(x, y);
            var spaceCount = CalculateConsoleSpaces((x, y), (xTarget, yTarget));
            Console.Write(new string(value, spaceCount));
        }

        /// <summary>
        /// Replace buffer with a space and reset postion 
        /// </summary>
        /// <param name="name">name of buffer memory</param>
        /// <param name="left">replace until this column, current by default</param>
        /// <param name="top">replace until this line, current by default</param>
        public static void ClearBufferFrom(string name, int? left = default, int? top = default) {
            if (!_memo.ContainsKey(name)) { return; }
            var (x, y) = _memo[name];
            ReplaceBufferFrom(name, ' ', left, top);
            Console.SetCursorPosition(x, y);
        }

        private static int CalculateConsoleSpaces((int x, int y) p1, (int x, int y) p2) {
            if (p2.y < p1.y || (p2.y == p1.y && p2.x <= p1.x)) { return 0; }
            if (p1.y == p2.y) { return p2.x - p1.x; }

            var fullLines = p2.y - p1.y - 1;
            var space = (fullLines < 0 ? 0 : fullLines) * Console.BufferWidth;
            space += Console.BufferWidth - p2.x;
            space += Console.BufferWidth - p1.x;

            return space;

        }
    }
}