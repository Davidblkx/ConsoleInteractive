using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using static ConsoleInteractive.ConsoleBuffer;

namespace ConsoleInteractive.Demo
{
    public static class BufferDemo
    {
        public static Command BuildBufferCommand() {
            var cmd = new Command("buffer", "buffer management demo")
            {
                Handler = CommandHandler.Create(() => StartDemo())
            };
            cmd.AddAlias("b");

            return cmd;
        }

        private static void StartDemo() {
            var token = "clear";
            Console.WriteLine($"Write {token} to clear the buffer from this message, or exit to close");

            // Save current buffer position
            MemoriseBufferPosition("BATATAS");

            do { // read inputs until keyword is match

            var res = Console.ReadLine();

            // if clear is found, replace buffer
            if (res == token) { ClearBufferFrom("BATATAS"); }
            if (res == "exit") { return; }

            } while(true);
        }
    }
}