using System.Linq;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using ConsoleInteractive.Components;

namespace ConsoleInteractive.Demo
{
    public static class SelectionDemo
    {
        public static Command BuildSelectionCommand() {
            var cmd = new Command("selection", "Console selections demo")
            {
                Handler = CommandHandler.Create(() => StartDemo())
            };
            cmd.AddAlias("s");

            return cmd;
        }

        private static async void StartDemo() {
            const string NAME = "#BUFFER_SELECTION123#";
            ConsoleBuffer.MemoriseBufferPosition(NAME);

            Console.WriteLine("Select 1 option");
            var selection = InputSelection.From<TestClass>()
                .AddOption(new TestClass { Name = "option1" })
                .AddOption(new TestClass { Name = "option2" })
                .AddOption(new TestClass { Name = "option3" })
                .AddOption(new TestClass { Name = "option4" });

            var result1 = await selection.RequestInput();
            Console.WriteLine($"Selected => {result1.First()}");
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);

            Console.WriteLine("Select 1 to 5 options");
            var selection2 = InputSelection.From<TestClass>()
                .SetMaxSelected(5)
                .AddOption(new TestClass { Name = "option1" })
                .AddOption(new TestClass { Name = "option2" })
                .AddOption(new TestClass { Name = "option3" })
                .AddOption(new TestClass { Name = "option4" })
                .AddOption(new TestClass { Name = "option5" })
                .AddOption(new TestClass { Name = "option6" })
                .AddOption(new TestClass { Name = "option7" })
                .AddOption(new TestClass { Name = "option8" })
                .AddOption(new TestClass { Name = "option9" })
                .AddOption(new TestClass { Name = "option10" })
                .AddOption(new TestClass { Name = "option11" });

            var result2 = await selection2.RequestInput();
            foreach(var r in result2) {
                Console.WriteLine($"Selected => {r}");
            }
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);

            Console.WriteLine("Also works with ENUM");
            var result3 = await InputSelection
                .FromEnum<EnumTest>()
                .RequestInput();
            foreach(var r in result3) {
                Console.WriteLine($"Selected => {(int)r}");
            }
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);
        }

        private enum EnumTest {
            Data1 = 1,
            Data2 = 3,
            Batatas = 12,
        }

        private class TestClass {
            public string Name { get; set; } = "";
            public List<string> Values { get; set; } =
                new List<string>();

            public override string ToString() {
                return Name;
            }
        }
    }
}