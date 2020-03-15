using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using ConsoleInteractive.Selection;
using System.Linq;

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

        private static void StartDemo() {
            const string NAME = "#BUFFER_SELECTION123#";
            ConsoleBuffer.MemoriseBufferPosition(NAME);

            Console.WriteLine("Select 1 option");
            var options = new SelectionGroup<TestClass>()
                .Add(new TestClass { Name = "option1" })
                .Add(new TestClass { Name = "option2" })
                .Add(new TestClass { Name = "option3" })
                .Add(new TestClass { Name = "option4" });

            var result1 = ConsoleI.Select(options);
            Console.WriteLine($"Selected => {result1}");
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);

            Console.WriteLine("Select 1 to 5 options");
            var options2 = new SelectionGroup<TestClass>()
                .Add(new TestClass { Name = "option1" })
                .Add(new TestClass { Name = "option2" })
                .Add(new TestClass { Name = "option3" })
                .Add(new TestClass { Name = "option4" })
                .Add(new TestClass { Name = "option5" })
                .Add(new TestClass { Name = "option6" })
                .Add(new TestClass { Name = "option7" })
                .Add(new TestClass { Name = "option8" })
                .Add(new TestClass { Name = "option9" })
                .Add(new TestClass { Name = "option10" })
                .Add(new TestClass { Name = "option11" });

            var result2 = ConsoleI.Select(options2, 5);
            foreach(var r in result2) {
                Console.WriteLine($"Selected => {r}");
            }
            ConsoleI.AwaitContinue();
            ConsoleBuffer.ClearBufferFrom(NAME);

            Console.WriteLine("Also works with ENUM");
            var result3 = ConsoleI.Select<EnumTest>(3);
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