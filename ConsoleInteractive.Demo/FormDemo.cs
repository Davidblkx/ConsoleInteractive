using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace ConsoleInteractive.Demo
{
    public static class FormDemo
    {
        public static Command BuildFormCommand() {
            var cmd = new Command("form", "Form demo")
            {
                Handler = CommandHandler.Create(() => StartDemo())
            };
            cmd.AddAlias("f");

            return cmd;
        }

        public static async Task StartDemo() {
            ConsoleI.AwaitContinue();
        }
    }

    public class TestFormClass {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<string> WorkTitles { get; set; }

        public override string ToString() {
            return $"{Name} ({Age}) => {string.Join(';', WorkTitles)}";
        }
    }
}