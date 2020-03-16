using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using ConsoleInteractive.Forms;

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
            var result = await ConsoleI.RequestForm<TestFormClass>();
            Console.WriteLine(result);
            ConsoleI.AwaitContinue();
        }
    }

    public class TestFormClass {
        [FormField("What is your name")]
        public string Name { get; set; }
        [FormField("What is your age", 30)]
        public int Age { get; set; }
        [FormField("Can list your work titles (separate by ';')")]
        public List<string> WorkTitles { get; set; }

        public override string ToString() {
            return $"{Name} ({Age}) => {string.Join(';', WorkTitles)}";
        }
    }
}